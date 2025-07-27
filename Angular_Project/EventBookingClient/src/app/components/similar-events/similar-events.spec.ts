import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { SimilarEvents } from './similar-events';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/Notification/notification-service';
import { TicketTypeEnum, EventStatus, EventCategory, EventTypeEnum } from '../../models/enum';
import { AppEvent } from '../../models/event.model';

describe('SimilarEvents Component', () => {
  let component: SimilarEvents;
  let fixture: ComponentFixture<SimilarEvents>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockNotify: jasmine.SpyObj<NotificationService>;

  // Properly initialized mockEvent with numeric ticket quantities
  const mockEvent: AppEvent = {
    id: '1',
    title: 'Sample Event',
    description: 'Test event',
    location: 'City Hall',
    eventDate: new Date(),
    eventStatus: EventStatus.Active,
    images: ['image1.jpg'],           // add required images array
    category: EventCategory.Music,                // add category string or enum
    eventType: EventTypeEnum.NonSeatable,             // add eventType string or enum
    bookedSeats: [],                // add bookedSeatsts array (empty array if no booked seats)
    ticketTypes: [
      {
        id: '101',
        typeName: TicketTypeEnum.Regular,
        price: 100,
        totalQuantity: 50,
        bookedQuantity: 10,
        description: '',
        isDeleted: false,
        imageUrl: ''
      }
    ]
  };


  beforeEach(async () => {
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl'], { url: '/user' });
    mockNotify = jasmine.createSpyObj('NotificationService', ['info']);

    // Return a resolved Promise so .then() works
    mockRouter.navigateByUrl.and.returnValue(Promise.resolve(true));

    await TestBed.configureTestingModule({
      imports: [SimilarEvents],
      providers: [
        { provide: Router, useValue: mockRouter },
        { provide: NotificationService, useValue: mockNotify }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(SimilarEvents);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should navigate to event page when event is active', fakeAsync(() => {
    component.GetEventById(mockEvent);
    tick();
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/user/events/1');
  }));

  it('should calculate total booked tickets', () => {
    const booked = component.getTotalBooked(mockEvent);
    expect(booked).toBe(10);
  });

  it('should calculate total available tickets', () => {
    const available = component.getTotalAvailable(mockEvent);
    expect(available).toBe(50);
  });

  it('should convert ticket enum to string', () => {
    const result = component.ticketTypeToString(TicketTypeEnum.Regular);
    expect(result).toBe('Regular');
  });
});
