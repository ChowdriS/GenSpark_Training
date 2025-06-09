using Moq;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using EventBookingApi.Interface;
using System.Text;

namespace EventBookingTest.Test.ServiceTest;

public class TicketServiceTest
{
    private Mock<IRepository<Guid, Ticket>> _ticketRepo;
    private Mock<IRepository<Guid, Event>> _eventRepo;
    private Mock<IRepository<Guid, TicketType>> _ticketTypeRepo;
    private Mock<IRepository<Guid, User>> _userRepo;
    private Mock<IRepository<Guid, BookedSeat>> _bookedSeatRepo;
    private Mock<IRepository<Guid, Payment>> _paymentRepo;
    private Mock<INotificationService> _notificationService;
    private TicketService _service;

    [SetUp]
    public void Setup()
    {
        _ticketRepo = new Mock<IRepository<Guid, Ticket>>();
        _eventRepo = new Mock<IRepository<Guid, Event>>();
        _ticketTypeRepo = new Mock<IRepository<Guid, TicketType>>();
        _userRepo = new Mock<IRepository<Guid, User>>();
        _bookedSeatRepo = new Mock<IRepository<Guid, BookedSeat>>();
        _paymentRepo = new Mock<IRepository<Guid, Payment>>();
        _notificationService = new Mock<INotificationService>();

        _service = new TicketService(
            _ticketRepo.Object,
            _eventRepo.Object,
            _ticketTypeRepo.Object,
            _userRepo.Object,
            _bookedSeatRepo.Object,
            _paymentRepo.Object,
            _notificationService.Object
        );
    }

    [Test]
    public async Task BookSeatableTicket_Success()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        var dto = new TicketBookRequestDTO
        {
            EventId = eventId,
            TicketTypeId = ticketTypeId,
            Quantity = 2,
            SeatNumbers = new List<int> { 1, 2 },
            Payment = new PaymentRequestDTO
            {
                PaymentType = PaymentTypeEnum.CreditCard,
                TransactionId = Guid.NewGuid(),
                Amount = 200
            }
        };

        var newEvent = new Event
        {
            Id = eventId,
            Title = "Concert",
            EventType = EventType.Seatable,
            EventStatus = EventStatus.Active,
            IsDeleted = false
        };

        var ticketType = new TicketType
        {
            Id = ticketTypeId,
            Price = 100,
            TotalQuantity = 10,
            BookedQuantity = 0,
            TypeName = TicketTypeEnum.VIP
        };

        var payment = new Payment { Id = paymentId };
        var ticket = new Ticket { Id = ticketId };

        _eventRepo.Setup(r => r.GetById(eventId)).ReturnsAsync(newEvent);
        _ticketTypeRepo.Setup(r => r.GetById(ticketTypeId)).ReturnsAsync(ticketType);
        _bookedSeatRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<BookedSeat>());
        _paymentRepo.Setup(r => r.Add(It.IsAny<Payment>())).ReturnsAsync(payment);
        _ticketRepo.Setup(r => r.Add(It.IsAny<Ticket>())).ReturnsAsync(ticket);
        _ticketTypeRepo.Setup(r => r.Update(It.IsAny<Guid>(), It.IsAny<TicketType>())).ReturnsAsync(new TicketType());

        // Act
        var result = await _service.BookTicket(dto, userId);

        // Assert
        Assert.That(result.EventTitle, Is.EqualTo("Concert"));
        Assert.That(result.Quantity, Is.EqualTo(2));
        Assert.That(result.SeatNumbers, Is.EquivalentTo(new List<int> { 1, 2 }));
        Assert.That(result.TicketType, Is.EqualTo("VIP"));
        _notificationService.Verify(n => n.NotifyUser(userId, It.IsAny<string>(), "TicketBooked"), Times.Once);
    }

    [Test]
    public void BookSeatableTicket_ThrowsWhenSeatsUnavailable()
    {
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bookedSeatNumber = 5;

        var @event = new Event { Id = eventId, EventType = EventType.Seatable };
        var ticketType = new TicketType { Id = ticketTypeId, EventId = eventId, TotalQuantity = 100, BookedQuantity = 90 };

        var bookedSeat = new BookedSeat
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            SeatNumber = bookedSeatNumber
        };

        var dto = new TicketBookRequestDTO
        {
            EventId = eventId,
            TicketTypeId = ticketTypeId,
            Quantity = 1,
            SeatNumbers = new List<int> { bookedSeatNumber },
            Payment = new PaymentRequestDTO
            {
                Amount = 100,
                TransactionId = Guid.NewGuid(),
                PaymentType = PaymentTypeEnum.UPI
            }
        };

        _eventRepo.Setup(x => x.GetById(eventId)).ReturnsAsync(@event);
        _ticketTypeRepo.Setup(x => x.GetById(ticketTypeId)).ReturnsAsync(ticketType);
        _bookedSeatRepo.Setup(x => x.GetAll()).ReturnsAsync(new List<BookedSeat> { bookedSeat });

        var ex = Assert.ThrowsAsync<Exception>(async () => await _service.BookTicket(dto,userId));
        Assert.That(ex!.Message, Does.Contain("already booked"));
    }

    [Test]
    public async Task BookNonSeatableTicket_Success()
    {
        // Arrange
        var dto = new TicketBookRequestDTO
        {
            EventId = Guid.NewGuid(),
            TicketTypeId = Guid.NewGuid(),
            Quantity = 3,
            Payment = new PaymentRequestDTO()
        };

        var newEvent = new Event
        {
            EventType = EventType.NonSeatable,
            EventStatus = EventStatus.Active,
            Title = "Festival",
            IsDeleted = false
        };

        var ticketType = new TicketType
        {
            TotalQuantity = 5,
            BookedQuantity = 0,
            Price = 50,
            TypeName = TicketTypeEnum.Regular
        };

        var mockTicket = new Ticket { Id = Guid.NewGuid() };
        _eventRepo.Setup(r => r.GetById(dto.EventId)).ReturnsAsync(newEvent);
        _ticketTypeRepo.Setup(r => r.GetById(dto.TicketTypeId)).ReturnsAsync(ticketType);
        _ticketRepo.Setup(r => r.Add(It.IsAny<Ticket>())).ReturnsAsync(mockTicket);

        // Act
        var result = await _service.BookTicket(dto, Guid.NewGuid());

        // Assert
        Assert.That(result.EventTitle, Is.EqualTo("Festival"));
        Assert.That(result.Quantity, Is.EqualTo(3));
        Assert.That(result.SeatNumbers, Is.Null);
    }

    [Test]
    public void BookTicket_ThrowsWhenEventInactive()
    {
        // Arrange
        var dto = new TicketBookRequestDTO { EventId = Guid.NewGuid() };
        var newEvent = new Event { EventStatus = EventStatus.Cancelled, IsDeleted = false };

        _eventRepo.Setup(r => r.GetById(dto.EventId)).ReturnsAsync(newEvent);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _service.BookTicket(dto, Guid.NewGuid()));
    }

    [Test]
    public async Task CancelTicket_Success()
    {
        // Arrange
    var userId = Guid.NewGuid();
    var ticketId = Guid.NewGuid();
    var eventId = Guid.NewGuid();
    var ticketTypeId = Guid.NewGuid();
    var paymentId = Guid.NewGuid();

    var ticketType = new TicketType
    {
        Id = ticketTypeId,
        TotalQuantity = 100,
        BookedQuantity = 1,
        Price = 200,
        TypeName = TicketTypeEnum.Regular
    };

    var payment = new Payment
    {
        Id = paymentId,
        PaymentStatus = PaymentStatusEnum.Paid
    };

    var bookedSeats = new List<BookedSeat>
    {
        new BookedSeat
        {
            Id = Guid.NewGuid(),
            SeatNumber = 1,
            EventId = eventId,
            TicketId = ticketId,
            BookedSeatStatus = BookedSeatStatus.Booked
        }
    };

    var ticket = new Ticket
    {
        Id = ticketId,
        UserId = userId,
        EventId = eventId,
        TicketTypeId = ticketTypeId,
        BookedQuantity = 1,
        TicketType = ticketType,
        Status = TicketStatus.Booked,
        BookedSeats = bookedSeats,
        PaymentId = paymentId
    };

    var eventObj = new Event
    {
        Id = eventId,
        Title = "Sample Event",
        EventType = EventType.Seatable
    };

    _ticketRepo.Setup(x => x.GetById(ticketId)).ReturnsAsync(ticket);
    _ticketRepo.Setup(x => x.Update(ticketId, It.IsAny<Ticket>())).ReturnsAsync(ticket);

    _ticketTypeRepo.Setup(x => x.GetById(ticketTypeId)).ReturnsAsync(ticketType);
    _ticketTypeRepo.Setup(x => x.Update(ticketTypeId, It.IsAny<TicketType>())).ReturnsAsync(ticketType);

    _eventRepo.Setup(x => x.GetById(eventId)).ReturnsAsync(eventObj);

    _paymentRepo.Setup(x => x.GetById(paymentId)).ReturnsAsync(payment);
    _paymentRepo.Setup(x => x.Update(paymentId, It.IsAny<Payment>())).ReturnsAsync(payment);

    _bookedSeatRepo.Setup(x => x.GetAll()).ReturnsAsync(bookedSeats);
    _bookedSeatRepo.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<BookedSeat>())).ReturnsAsync(new BookedSeat());

    // Act + Assert
    Assert.DoesNotThrowAsync(async () => await _service.CancelTicket(ticketId, userId));

    }


    [Test]
    public void CancelTicket_ThrowsWhenNotOwner()
    {
        // Arrange
        var ticket = new Ticket { UserId = Guid.NewGuid() };
        _ticketRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(ticket);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _service.CancelTicket(Guid.NewGuid(), Guid.NewGuid())
        );
    }

    [Test]
    public async Task GetMyTickets_ReturnsOnlyUsersTickets()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tickets = new List<Ticket>
        {
            new Ticket { UserId = userId, Status = TicketStatus.Booked },
            new Ticket { UserId = Guid.NewGuid(), Status = TicketStatus.Booked },
            new Ticket { UserId = userId, Status = TicketStatus.Cancelled } // Should be excluded
        };

        _ticketRepo.Setup(r => r.GetAll()).ReturnsAsync(tickets);
        _eventRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new Event());
        _ticketTypeRepo.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new TicketType());

        // Act
        var result = await _service.GetMyTickets(userId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task ExportTicketAsPdf_GeneratesCorrectContent()
    {
        var ticketId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var ticket = new Ticket
        {
            Id = ticketId,
            UserId = userId,
            Event = new Event { Title = "Sample Event", EventDate = DateTime.Now },
            TicketType = new TicketType { Price = 100, TypeName = TicketTypeEnum.Regular },
            BookedQuantity = 2,
            Payment = new Payment
            {
                Amount = 200,
                PaymentType = PaymentTypeEnum.UPI,
                PaymentStatus = PaymentStatusEnum.Paid,
                TransactionId = Guid.NewGuid(),
                PaidAt = DateTime.UtcNow
            },
            BookedAt = DateTime.UtcNow
        };

        _ticketRepo.Setup(r => r.GetById(ticketId)).ReturnsAsync(ticket);

        var content = await _service.ExportTicketAsPdf(ticketId, userId);

        Assert.IsNotNull(content);
        Assert.That(content.Length, Is.GreaterThan(0));
    }
}