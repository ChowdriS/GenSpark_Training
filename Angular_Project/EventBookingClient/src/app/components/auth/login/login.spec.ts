// login.component.spec.ts

import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Login } from './login';
import { Auth } from '../../../services/Auth/auth';
import { NotificationService } from '../../../services/Notification/notification-service';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let mockAuthService: jasmine.SpyObj<Auth>;
  let mockNotify: jasmine.SpyObj<NotificationService>;
  let router: Router;
  function createFakeTokenWithRole(role: string): string {
    const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
    const payload = btoa(JSON.stringify({ role }));
    const signature = 'fake-signature';
    return `${header}.${payload}.${signature}`;
  }


  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('Auth', ['login', 'getToken', 'setToken']);
    mockNotify = jasmine.createSpyObj('NotificationService', ['success', 'error']);

    await TestBed.configureTestingModule({
      imports: [
        Login,              
        ReactiveFormsModule,
        RouterTestingModule 
      ],
      providers: [
        { provide: Auth, useValue: mockAuthService },
        { provide: NotificationService, useValue: mockNotify }
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    spyOn(router, 'navigate'); 
    component.ngOnInit();     
  });

  it('should create component and initialize form invalid', () => {
    expect(component).toBeTruthy();
    expect(component.loginForm).toBeDefined();
    expect(component.loginForm.valid).toBeFalse();
  });

  it('should validate email and password fields properly', () => {
    const form = component.loginForm;

    form.get('email')!.setValue('invalid-email');
    form.get('password')!.setValue('123');
    expect(form.invalid).toBeTrue();

    form.get('email')!.setValue('user@example.com');
    form.get('password')!.setValue('123456');
    expect(form.valid).toBeTrue();
  });

it('should call AuthService.login and set token, navigate, notify success on successful login', fakeAsync(() => {
  const fakeToken = createFakeTokenWithRole('User');  
  
  const apiResponse = {
    $id : "0",
    success: true,
    message: '',
    data: { token: fakeToken },
    errors: null
  };

  component.loginForm.setValue({ email: 'test@example.com', password: '123456' });
  mockAuthService.login.and.returnValue(of(apiResponse));
  mockAuthService.getToken.and.returnValue(fakeToken);

  component.onLogin();
  tick();

  expect(mockAuthService.login).toHaveBeenCalledWith({ email: 'test@example.com', password: '123456' });
  expect(mockAuthService.setToken).toHaveBeenCalledWith(fakeToken);
  expect(router.navigate).toHaveBeenCalled(); // Because roleBasedRoute calls navigate based on role
  expect(mockNotify.success).toHaveBeenCalledWith('Login Success');
}));


  it('should notify error if login response success is false', fakeAsync(() => {
    const response = { $id : "0",success: false, message: 'Invalid credentials', data: null, errors: null };

    component.loginForm.setValue({ email: 'fail@example.com', password: '123456' });
    mockAuthService.login.and.returnValue(of(response));

    component.onLogin();
    tick();

    expect(mockNotify.error).toHaveBeenCalledWith('Invalid credentials');
  }));

  it('should notify error with API error message on login error', fakeAsync(() => {
    const errorResponse = { error: { errors: { message: 'Server error occurred' } } };

    component.loginForm.setValue({ email: 'error@example.com', password: '123456' });
    mockAuthService.login.and.returnValue(throwError(() => errorResponse));

    component.onLogin();
    tick();

    expect(mockNotify.error).toHaveBeenCalledWith('Server error occurred');
  }));
});
