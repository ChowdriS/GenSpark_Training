using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using Moq;
using NUnit.Framework;

namespace EventBookingApi.Test.ServiceTest;

public class EventServiceTest
{
    private Mock<IRepository<Guid, Event>> _eventRepoMock;
    private Mock<IRepository<Guid, TicketType>> _ticketTypeRepoMock;
    private Mock<IRepository<Guid, User>> _userRepoMock;
    private EventService _eventService;

    [SetUp]
    public void Setup()
    {
        _eventRepoMock = new Mock<IRepository<Guid, Event>>();
        _ticketTypeRepoMock = new Mock<IRepository<Guid, TicketType>>();
        _userRepoMock = new Mock<IRepository<Guid, User>>();

        _eventService = new EventService(_eventRepoMock.Object, _ticketTypeRepoMock.Object, _userRepoMock.Object);
    }

    [Test]
    public async Task GetAllEvents_ReturnsEvents()
    {
        var eventList = new List<Event>
        {
            new Event { Id = Guid.NewGuid(), Title = "Test Event", IsDeleted = false }
        };

        _eventRepoMock.Setup(r => r.GetAll()).ReturnsAsync(eventList);

        var result = await _eventService.GetAllEvents();

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Title, Is.EqualTo("Test Event"));
    }

    [Test]
    public async Task GetEventById_ValidId_ReturnsEvent()
    {
        var id = Guid.NewGuid();
        var ev = new Event { Id = id, Title = "Event", IsDeleted = false };

        _eventRepoMock.Setup(r => r.GetById(id)).ReturnsAsync(ev);

        var result = await _eventService.GetEventById(id);

        Assert.That(result.Title, Is.EqualTo("Event"));
    }

    [Test]
    public void GetEventById_InvalidId_ThrowsException()
    {
        _eventRepoMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Event)null);

        Assert.ThrowsAsync<Exception>(() => _eventService.GetEventById(Guid.NewGuid()));
    }

    [Test]
    public async Task FilterEvents_ByCity_ReturnsMatchingEvents()
    {
        var events = new List<Event>
        {
            new Event { Title = "E1", Description = "Chennai", EventDate = DateTime.Today, IsDeleted = false },
            new Event { Title = "E2", Description = "Delhi", EventDate = DateTime.Today, IsDeleted = false }
        };

        _eventRepoMock.Setup(r => r.GetAll()).ReturnsAsync(events);

        var result = await _eventService.FilterEvents("Chennai", null);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Description, Is.EqualTo("Chennai"));
    }

    [Test]
    public async Task GetManagedEventsByUserId_ReturnsEvents()
    {
        var userId = Guid.NewGuid();
        var events = new List<Event>
        {
            new Event { ManagerId = userId, IsDeleted = false }
        };

        _eventRepoMock.Setup(r => r.GetAll()).ReturnsAsync(events);

        var result = await _eventService.GetManagedEventsByUserId(userId);

        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task AddEvent_WithTickets_AddsEventAndTickets()
    {
        var managerId = Guid.NewGuid();
        var newEventId = Guid.NewGuid();

        _userRepoMock.Setup(u => u.GetById(managerId)).ReturnsAsync(new User { Id = managerId });

        _eventRepoMock.Setup(e => e.Add(It.IsAny<Event>()))
            .ReturnsAsync((Event e) => { e.Id = newEventId; return e; });

        var dto = new EventAddRequestDTO
        {
            Title = "New Event",
            Description = "Test Desc",
            EventDate = DateTime.Today,
            EventType = EventType.Seatable,
            ManagerId = managerId,
            TicketTypes = new List<TicketTypeAddRequestDTO>
            {
                new TicketTypeAddRequestDTO
                {
                    TypeName = TicketTypeEnum.VIP,
                    Price = 100,
                    TotalQuantity = 10,
                    Description = "VIP"
                }
            }
        };

        var result = await _eventService.AddEvent(dto);

        _ticketTypeRepoMock.Verify(t => t.Add(It.IsAny<TicketType>()), Times.Once);
        Assert.That(result.Title, Is.EqualTo("New Event"));
    }

    [Test]
    public async Task UpdateEvent_ValidUpdate_ChangesFields()
    {
        var eventId = Guid.NewGuid();
        var ev = new Event
        {
            Id = eventId,
            Title = "Old Title",
            Description = "Old",
            EventDate = DateTime.Today,
            EventType = EventType.Seatable,
            EventStatus = EventStatus.Active
        };

        _eventRepoMock.Setup(r => r.GetById(eventId)).ReturnsAsync(ev);
        _eventRepoMock.Setup(r => r.Update(eventId, It.IsAny<Event>())).ReturnsAsync((Guid _, Event updated) => updated);

        var dto = new EventUpdateRequestDTO
        {
            Title = "Updated Title",
            Description = "Updated",
            EventDate = DateTime.Today.AddDays(1),
            EventType = EventType.NonSeatable,
            EventStatus = EventStatus.Completed
        };

        var result = await _eventService.UpdateEvent(eventId, dto);

        Assert.That(result.Title, Is.EqualTo("Updated Title"));
        Assert.That(result.EventStatus, Is.EqualTo("Completed"));
    }

    [Test]
    public async Task DeleteEvent_ValidId_SetsIsDeletedAndCancels()
    {
        var eventId = Guid.NewGuid();
        var ev = new Event { Id = eventId, Title = "ToDelete", IsDeleted = false };

        _eventRepoMock.Setup(r => r.GetById(eventId)).ReturnsAsync(ev);
        _eventRepoMock.Setup(r => r.Update(eventId, It.IsAny<Event>())).ReturnsAsync((Guid _, Event e) => e);

        var result = await _eventService.DeleteEvent(eventId);

        Assert.That(result.EventStatus, Is.EqualTo("Cancelled"));
    }
}
