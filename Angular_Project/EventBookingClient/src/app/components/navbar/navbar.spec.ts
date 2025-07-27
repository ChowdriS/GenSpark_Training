import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Navbar } from './navbar';
import { UserService } from '../../services/User/user-service';
import { Auth } from '../../services/Auth/auth';
import { NotificationService } from '../../services/Notification/notification-service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ApiResponse } from '../../models/api-response.model';
import { RouterTestingModule } from '@angular/router/testing';

describe('Navbar Component', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockAuthService: jasmine.SpyObj<Auth>;
  let mockNotify: jasmine.SpyObj<NotificationService>;
  let router: Router;

  const fakeUser = {
    email: 'test@example.com',
    username: 'testuser',
    role: 'User'
  };

  beforeEach(async () => {
    mockUserService = jasmine.createSpyObj('UserService', ['getUserDetails']);
    mockAuthService = jasmine.createSpyObj('Auth', ['getToken']);
    mockNotify = jasmine.createSpyObj('NotificationService', ['error']);

    await TestBed.configureTestingModule({
      imports: [
        Navbar,
        RouterTestingModule  // for routerLink directives and routing deps
      ],
      providers: [
        { provide: UserService, useValue: mockUserService },
        { provide: Auth, useValue: mockAuthService },
        { provide: NotificationService, useValue: mockNotify }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
  });

  it('should create component and initialize with null user and menu closed', () => {
    expect(component).toBeTruthy();
    expect(component.user()).toBeNull();
    expect(component.menuopen()).toBeFalse();
  });

  it('should fetch user details on ngOnInit success', fakeAsync(() => {
    const apiResponse: ApiResponse = { $id : "0",success: true, message: '', data: fakeUser, errors: null };
    mockUserService.getUserDetails.and.returnValue(of(apiResponse));

    component.ngOnInit();
    tick();

    expect(mockUserService.getUserDetails).toHaveBeenCalled();
    expect(component.user()).toEqual(fakeUser);
  }));

  it('should notify error when fetching user details fails', fakeAsync(() => {
    mockUserService.getUserDetails.and.returnValue(throwError(() => new Error('Network Error')));

    component.ngOnInit();
    tick();

    expect(mockNotify.error).toHaveBeenCalledWith('Failed to fetch your Data');
  }));

  it('should toggle menuopen signal when menuOpen() called', () => {
    expect(component.menuopen()).toBe(false);
    component.menuOpen();
    expect(component.menuopen()).toBe(true);
    component.menuOpen();
    expect(component.menuopen()).toBe(false);
  });

  it('should clear token, close menu and navigate to login on logout', () => {
    spyOn(localStorage, 'removeItem').and.callThrough();

    component.menuopen.set(true);
    component.logout();

    expect(localStorage.removeItem).toHaveBeenCalledWith('token');
    expect(component.menuopen()).toBe(false);
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should navigate to profile route based on user role in token', () => {
    // Helper to create a JWT with role
    function createFakeTokenWithRole(role: string): string {
      const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
      const payload = btoa(JSON.stringify({ role }));
      return `${header}.${payload}.sig`;
    }

    const event = jasmine.createSpyObj('event', ['preventDefault']);
    // For User role
    mockAuthService.getToken.and.returnValue(createFakeTokenWithRole('User'));
    component.navigateToProfile(event);
    expect(event.preventDefault).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/user/profile']);
    expect(component.menuopen()).toBe(false);

    // For Manager role
    mockAuthService.getToken.and.returnValue(createFakeTokenWithRole('Manager'));
    component.navigateToProfile(event);
    expect(router.navigate).toHaveBeenCalledWith(['/manager/profile']);
  });

});
