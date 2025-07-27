// landing-page.component.spec.ts

import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LandingPage } from './landing-page';
import { Auth } from '../../services/Auth/auth';
import { Router } from '@angular/router';
import { EventService } from '../../services/Event/event.service';
import { RouterTestingModule } from '@angular/router/testing';
import { of, throwError } from 'rxjs';

describe('LandingPage Component', () => {
  let component: LandingPage;
  let fixture: ComponentFixture<LandingPage>;
  let mockAuth: jasmine.SpyObj<Auth>;
  let mockEventService: jasmine.SpyObj<EventService>;
  let router: Router;

  function createFakeTokenWithRole(role: string): string {
    const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }));
    const payload = btoa(JSON.stringify({ role }));
    const signature = 'fake-signature';
    return `${header}.${payload}.${signature}`;
  }

  beforeEach(async () => {
    mockAuth = jasmine.createSpyObj('Auth', ['getToken']);
    mockEventService = jasmine.createSpyObj('EventService', ['getAllEventImages']);

    await TestBed.configureTestingModule({
      imports: [
        LandingPage,
        RouterTestingModule.withRoutes([])  // Provides Router and ActivatedRoute
      ],
      providers: [
        { provide: Auth, useValue: mockAuth },
        { provide: EventService, useValue: mockEventService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LandingPage);
    component = fixture.componentInstance;

    router = TestBed.inject(Router);
    spyOn(router, 'navigate');  // Spy on router navigation
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should navigate to /user if token role is User', () => {
      mockAuth.getToken.and.returnValue(createFakeTokenWithRole('User'));
      component.ngOnInit();
      expect(router.navigate).toHaveBeenCalledWith(['/user']);
    });

    it('should navigate to /manager if token role is Manager', () => {
      mockAuth.getToken.and.returnValue(createFakeTokenWithRole('Manager'));
      component.ngOnInit();
      expect(router.navigate).toHaveBeenCalledWith(['/manager']);
    });

    it('should navigate to /admin if token role is Admin', () => {
      mockAuth.getToken.and.returnValue(createFakeTokenWithRole('Admin'));
      component.ngOnInit();
      expect(router.navigate).toHaveBeenCalledWith(['/admin']);
    });

    it('should NOT navigate if token is null', () => {
      mockAuth.getToken.and.returnValue(null);
      component.ngOnInit();
      expect(router.navigate).not.toHaveBeenCalled();
    });
  });


  describe('GetAllEventImages', () => {
    it('should set images signal on successful response', (done) => {
      const mockImages = [{ id: 1, url: 'url1' }, { id: 2, url: 'url2' }];
      mockEventService.getAllEventImages.and.returnValue(of({ $values: mockImages }));

      component.GetAllEventImages();

      setTimeout(() => {
        expect(component.images()).toEqual(mockImages);
        done();
      }, 0);
    });

    it('should not set images on error (and not throw)', () => {
      mockEventService.getAllEventImages.and.returnValue(throwError(() => new Error('Network error')));
      expect(() => component.GetAllEventImages()).not.toThrow();
      expect(component.images()).toBeNull();
    });
  });

  describe('startSlider', () => {
    beforeEach(() => {
      component.images.set([
        { id: 1, url: 'url1' },
        { id: 2, url: 'url2' },
        { id: 3, url: 'url3' }
      ]);
    });

    afterEach(() => {
      if (component.intervalId) {
        clearInterval(component.intervalId);
      }
    });

    it('should cycle currentIndex every 3500ms', fakeAsync(() => {
      component.startSlider();
      expect(component.currentIndex()).toBe(0);

      tick(3500);
      expect(component.currentIndex()).toBe(1);

      tick(3500);
      expect(component.currentIndex()).toBe(2);

      tick(3500);
      expect(component.currentIndex()).toBe(0);

      clearInterval(component.intervalId);
    }));
  });

  it('should set currentIndex on goToSlide', () => {
    component.goToSlide(2);
    expect(component.currentIndex()).toBe(2);
  });

});
