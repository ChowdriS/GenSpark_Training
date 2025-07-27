import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { AuthGuard, UserGuard, ManagerGuard, AdminGuard } from './auth-guard';
import { Auth } from '../services/Auth/auth';
import { SignalRService } from '../services/Notification/signalr-service';
import { Getrole } from '../misc/Token';

class MockRouter {
  navigate(path: any[]) {}
}
class MockAuth {
  getToken() { return null; }
}
class MockSignalRService {
  startConnection() {}
}


describe('AuthGuard', () => {
  let guard: AuthGuard;
  let auth: Auth;
  let router: Router;
  let signalR: SignalRService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: Auth, useClass: MockAuth },
        { provide: Router, useClass: MockRouter },
        { provide: SignalRService, useClass: MockSignalRService }
      ]
    });
    guard = TestBed.inject(AuthGuard);
    auth = TestBed.inject(Auth);
    router = TestBed.inject(Router);
    signalR = TestBed.inject(SignalRService);
    spyOn(signalR, 'startConnection');
    spyOn(router, 'navigate');
  });

  it('should activate if token exists', () => {
    spyOn(auth, 'getToken').and.returnValue('someToken');
    expect(guard.canActivate()).toBeTrue();
    expect(signalR.startConnection).toHaveBeenCalled();
    expect(router.navigate).not.toHaveBeenCalled();
  });
  it('should not activate if token does not exist', () => {
    spyOn(auth, 'getToken').and.returnValue(null);
    expect(guard.canActivate()).toBeFalse();
    expect(signalR.startConnection).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/default']);
  });
});

describe('UserGuard', () => {
  let guard: UserGuard;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        UserGuard,
        { provide: Router, useClass: MockRouter }
      ]
    });
    guard = TestBed.inject(UserGuard);
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
  });

  function fakeTokenWithRole(role: string) {
    // JWT with role, base64 encoding for test
    const header = btoa('{}');
    const payload = btoa(JSON.stringify({role}));
    return `${header}.${payload}.sig`;
  }

  it('should activate for User role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(fakeTokenWithRole('User'));
    expect(guard.canActivate()).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });
  it('should not activate for non-User role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(fakeTokenWithRole('Other'));
    expect(guard.canActivate()).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
  it('should not activate when no token', () => {
    spyOn(localStorage, 'getItem').and.returnValue(null);
    expect(guard.canActivate()).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});

describe('ManagerGuard', () => {
  let guard: ManagerGuard, router: Router;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ManagerGuard,
        { provide: Router, useClass: MockRouter }
      ]
    });
    guard = TestBed.inject(ManagerGuard);
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
  });
  it('should activate for Manager role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(btoa('{}') + '.' + btoa(JSON.stringify({role: 'Manager'})) + '.sig');
    expect(guard.canActivate()).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });
  it('should not activate for non-Manager role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(btoa('{}') + '.' + btoa(JSON.stringify({role: 'Other'})) + '.sig');
    expect(guard.canActivate()).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});

describe('AdminGuard', () => {
  let guard: AdminGuard, router: Router;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AdminGuard,
        { provide: Router, useClass: MockRouter }
      ]
    });
    guard = TestBed.inject(AdminGuard);
    router = TestBed.inject(Router);
    spyOn(router, 'navigate');
  });
  it('should activate for Admin role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(btoa('{}') + '.' + btoa(JSON.stringify({role: 'Admin'})) + '.sig');
    expect(guard.canActivate()).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });
  it('should not activate for non-Admin role', () => {
    spyOn(localStorage, 'getItem').and.returnValue(btoa('{}') + '.' + btoa(JSON.stringify({role: 'Other'})) + '.sig');
    expect(guard.canActivate()).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/login']);
  });
});
