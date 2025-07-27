// register.component.spec.ts

import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Register } from './register';
import { Auth } from '../../../services/Auth/auth';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/Notification/notification-service';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError } from 'rxjs';
import { ApiResponse } from '../../../models/api-response.model';

function createFakeTokenWithRole(role: string): string {
  const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
  const payload = btoa(JSON.stringify({ role }));
  const signature = 'fake-signature';
  return `${header}.${payload}.${signature}`;
}

describe('Register Component', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let mockAuthService: jasmine.SpyObj<Auth>;
  let mockNotify: jasmine.SpyObj<NotificationService>;
  let router: Router;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('Auth', ['register', 'getToken']);
    mockNotify = jasmine.createSpyObj('NotificationService', ['success', 'error']);

    await TestBed.configureTestingModule({
      imports: [
        Register,
        ReactiveFormsModule,
        RouterTestingModule.withRoutes([]), // Important for Router
      ],
      providers: [
        { provide: Auth, useValue: mockAuthService },
        { provide: NotificationService, useValue: mockNotify }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
    component.ngOnInit();
  });

  it('should create component and initialize form with default values', () => {
    expect(component).toBeTruthy();
    expect(component.registerForm).toBeDefined();

    const form = component.registerForm;
    expect(form.get('role')?.value).toBe('user');
    expect(form.get('username')?.value).toBe('');
    expect(form.get('email')?.value).toBe('');
    expect(form.get('password')?.value).toBe('');
    expect(form.get('confirmPassword')?.value).toBe('');
  });

  it('should validate password and confirmPassword match', () => {
    const form = component.registerForm;
    form.get('password')?.setValue('123456');
    form.get('confirmPassword')?.setValue('123456');
    expect(form.errors).toBeNull();

    form.get('confirmPassword')?.setValue('654321');
    expect(form.errors).toEqual({ mismatch: true });
  });

  it('should redirect based on existing token and role on init', () => {
    const tokenUser = createFakeTokenWithRole('User');
    mockAuthService.getToken.and.returnValue(tokenUser);

    component.ngOnInit();

    expect(router.navigate).toHaveBeenCalledWith(['/user']);
  });

  it('should call register and navigate on successful registration', fakeAsync(() => {
    component.registerForm.setValue({
      role: 'user',
      username: 'testuser',
      email: 'test@example.com',
      password: '123456',
      confirmPassword: '123456'
    });

    const apiResponse: ApiResponse = {
      $id : "0",
      success: true,
      message: 'Registered',
      data: null,
      errors: null
    };

    mockAuthService.register.and.returnValue(of(apiResponse));

    component.onRegister();
    tick();

    expect(mockAuthService.register).toHaveBeenCalledWith(component.registerForm.value);
    expect(mockNotify.success).toHaveBeenCalledWith('Registration successful!');
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  }));

  it('should show error notification on failed registration', fakeAsync(() => {
    component.registerForm.setValue({
      role: 'user',
      username: 'testuser',
      email: 'test@example.com',
      password: '123456',
      confirmPassword: '123456'
    });

    const apiResponse: ApiResponse = {
      $id : "0",
      success: false,
      message: 'Email already exists',
      data: null,
      errors: null
    };

    mockAuthService.register.and.returnValue(of(apiResponse));

    component.onRegister();
    tick();

    expect(mockAuthService.register).toHaveBeenCalled();
    expect(mockNotify.error).toHaveBeenCalledWith('Email already exists');
    expect(router.navigate).not.toHaveBeenCalled();
  }));

  it('should show error notification with API error message', fakeAsync(() => {
    component.registerForm.setValue({
      role: 'user',
      username: 'testuser',
      email: 'test@example.com',
      password: '123456',
      confirmPassword: '123456'
    });

    const errorResponse = { error: { errors: { message: 'Server error occurred' } } };
    mockAuthService.register.and.returnValue(throwError(() => errorResponse));

    component.onRegister();
    tick();

    expect(mockNotify.error).toHaveBeenCalledWith('Server error occurred');
  }));

  it('should show generic error notification if error message is missing', fakeAsync(() => {
    component.registerForm.setValue({
      role: 'user',
      username: 'testuser',
      email: 'test@example.com',
      password: '123456',
      confirmPassword: '123456'
    });

    const errorResponse = { error: { errors: {} } };
    mockAuthService.register.and.returnValue(throwError(() => errorResponse));

    component.onRegister();
    tick();

    expect(mockNotify.error).toHaveBeenCalledWith('An unknown error occurred.');
  }));
});
