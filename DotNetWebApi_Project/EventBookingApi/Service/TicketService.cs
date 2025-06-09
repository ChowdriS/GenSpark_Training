using System;
using System.Text;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;
public class TicketService : ITicketService
{
    private readonly IRepository<Guid, Ticket> _ticketRepository;
    private readonly IRepository<Guid, Event> _eventRepository;
    private readonly IRepository<Guid, TicketType> _ticketTypeRepository;
    private readonly IRepository<Guid, User> _userRepository;
    private readonly IRepository<Guid, BookedSeat> _bookedSeatRepository;
    private readonly IRepository<Guid, Payment> _paymentRepository;
    private readonly INotificationService _notificationService;

    public TicketService(
        IRepository<Guid, Ticket> ticketRepository,
        IRepository<Guid, Event> eventRepository,
        IRepository<Guid, TicketType> ticketTypeRepository,
        IRepository<Guid, User> userRepository,
        IRepository<Guid, BookedSeat> bookedSeatRepository,
        IRepository<Guid, Payment> paymentRepository,
        INotificationService notificationService)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _userRepository = userRepository;
        _bookedSeatRepository = bookedSeatRepository;
        _paymentRepository = paymentRepository;
        _notificationService = notificationService;
    }

    public async Task<TicketResponseDTO> BookTicket(TicketBookRequestDTO dto, Guid userId)
    {
        var eventObj = await _eventRepository.GetById(dto.EventId);
        if (eventObj == null || eventObj.IsDeleted || eventObj.EventStatus != EventStatus.Active)
            throw new Exception("Event not available");

        var ticketType = await _ticketTypeRepository.GetById(dto.TicketTypeId);
        if (ticketType == null)
            throw new Exception("Ticket type not found");

        if (eventObj.EventType == EventType.Seatable)
        {
            return await BookSeatableTicket(dto, userId, eventObj, ticketType);
        }
        else
        {
            return await BookNonSeatableTicket(dto, userId, eventObj, ticketType);
        }
    }
    private async Task<Payment> CreatePayment(PaymentRequestDTO? paymentDto, decimal amount)
    {
        var payment = new Payment
        {
            PaymentType = paymentDto!.PaymentType,
            Amount = amount,
            PaymentStatus = PaymentStatusEnum.Paid,
            TransactionId = paymentDto.TransactionId
        };

        return await _paymentRepository.Add(payment);
    }
    private async Task<TicketResponseDTO> BookSeatableTicket(
        TicketBookRequestDTO dto, Guid userId, Event eventObj, TicketType ticketType)
    {
        if (dto.SeatNumbers == null || dto.SeatNumbers.Count != dto.Quantity)
            throw new Exception("For seatable events, you must provide exactly as many seat numbers as the quantity");
        if (ticketType.TotalQuantity - ticketType.BookedQuantity <= 0)
        {
            await _notificationService.NotifyAdmins(
            $"Event '{eventObj.Title}' is sold out",
            "EventFull"
            );
            await _notificationService.NotifyEventManagers(
                eventObj.Id,
                $"Your event '{eventObj.Title}' is sold out", 
                "EventFull"
            );
            throw new Exception("No tickets available in that ticketType in the requested Event");
        }
        if (ticketType.TotalQuantity - ticketType.BookedQuantity < dto.Quantity)
        {
            throw new Exception("Not enough tickets available");
        }
        var existingSeats = (await _bookedSeatRepository.GetAll())
            .Where(bs => bs.EventId == eventObj.Id && 
                         dto.SeatNumbers.Contains(bs.SeatNumber) &&
                         bs.BookedSeatStatus == BookedSeatStatus.Booked)
            .ToList();

        if (existingSeats.Any())
        {
            var takenSeats = string.Join(", ", existingSeats.Select(s => s.SeatNumber));
            throw new Exception($"Seats {takenSeats} are already booked");
        }

        if (ticketType.TotalQuantity - ticketType.BookedQuantity < dto.Quantity)
            throw new Exception("Not enough tickets available");

        ticketType.BookedQuantity += dto.Quantity;
        var payment = await CreatePayment(dto.Payment, dto.Quantity * ticketType.Price);
        await _ticketTypeRepository.Update(ticketType.Id, ticketType);


        var ticket = new Ticket
        {
            UserId = userId,
            EventId = eventObj.Id,
            TicketTypeId = ticketType.Id,
            BookedQuantity = dto.Quantity,
            TotalPrice = dto.Quantity * ticketType.Price,
            Status = TicketStatus.Booked
        };

        var createdTicket = await _ticketRepository.Add(ticket);
        payment.TicketId = createdTicket.Id;
        await _paymentRepository.Update(payment.Id, payment);
        foreach (var seatNumber in dto.SeatNumbers)
        {
            var bookedSeat = new BookedSeat
            {
                EventId = eventObj.Id,
                TicketId = createdTicket.Id,
                SeatNumber = seatNumber,
                BookedSeatStatus = BookedSeatStatus.Booked
            };
            await _bookedSeatRepository.Add(bookedSeat);
        }
        await _notificationService.NotifyUser(
            userId, 
            $"Ticket booked for {eventObj.Title}", 
            "TicketBooked"
        );
        return new TicketResponseDTO
        {
            Id = createdTicket.Id,
            EventTitle = eventObj.Title ?? "",
            TicketType = ticketType.TypeName.ToString(),
            PricePerTicket = ticketType.Price,
            Quantity = dto.Quantity,
            BookedAt = createdTicket.BookedAt,
            SeatNumbers = dto.SeatNumbers
        };
    }

    private async Task<TicketResponseDTO> BookNonSeatableTicket(
        TicketBookRequestDTO dto, Guid userId, Event eventObj, TicketType ticketType)
    {
        if (dto.SeatNumbers != null && dto.SeatNumbers.Any())
            throw new Exception("Seat numbers should not be provided for non-seatable events");

        if (ticketType.TotalQuantity - ticketType.BookedQuantity < dto.Quantity)
            throw new Exception("Not enough tickets available");

        ticketType.BookedQuantity += dto.Quantity;
        await _ticketTypeRepository.Update(ticketType.Id, ticketType);

        var ticket = new Ticket
        {
            UserId = userId,
            EventId = eventObj.Id,
            TicketTypeId = ticketType.Id,
            BookedQuantity = dto.Quantity,
            TotalPrice = dto.Quantity * ticketType.Price,
            Status = TicketStatus.Booked
        };

        var createdTicket = await _ticketRepository.Add(ticket);
        await _notificationService.NotifyUser(
            userId, 
            $"Ticket booked for {eventObj.Title}", 
            "TicketBooked"
        );
        return new TicketResponseDTO
        {
            Id = createdTicket.Id,
            EventTitle = eventObj.Title ?? "",
            TicketType = ticketType.TypeName.ToString(),
            PricePerTicket = ticketType.Price,
            Quantity = dto.Quantity,
            BookedAt = createdTicket.BookedAt
        };
    }

    public async Task<TicketResponseDTO> CancelTicket(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Cannot cancel this ticket");
        
        if (ticket.Status != TicketStatus.Booked)
            throw new Exception("Ticket is not in a cancellable state");

        var eventObj = await _eventRepository.GetById(ticket.EventId);
        var ticketType = await _ticketTypeRepository.GetById(ticket.TicketTypeId);
        var payment = ticket.PaymentId.HasValue 
            ? await _paymentRepository.GetById(ticket.PaymentId.Value) 
            : null;
        List<BookedSeat>? bookedSeats = null;
        if (eventObj.EventType == EventType.Seatable)
        {
            bookedSeats = (await _bookedSeatRepository.GetAll())
                .Where(bs => bs.TicketId == ticketId)
                .ToList();

            foreach (var seat in bookedSeats)
            {
                seat.BookedSeatStatus = BookedSeatStatus.Cancelled;
                await _bookedSeatRepository.Update(seat.Id, seat);
            }
        }

        ticketType.BookedQuantity -= ticket.BookedQuantity;
        await _ticketTypeRepository.Update(ticketType.Id, ticketType);
        if (payment != null)
        {
            payment.PaymentStatus = PaymentStatusEnum.Refund;
            await _paymentRepository.Update(payment.Id, payment);
        }
        ticket.Status = TicketStatus.Cancelled;
        await _ticketRepository.Update(ticketId, ticket);
        await _notificationService.NotifyUser(
            userId, 
            $"Ticket canceled for {eventObj.Title}", 
            "TicketCancelled"
        );
        return new TicketResponseDTO
        {
            Id = ticket.Id,
            EventTitle = eventObj?.Title ?? "",
            TicketType = ticketType.TypeName.ToString(),
            PricePerTicket = ticketType.Price,
            Quantity = ticket.BookedQuantity,
            BookedAt = ticket.BookedAt,
            SeatNumbers = bookedSeats?.Select(t => t.SeatNumber).ToList(),
        };
    }

     public async Task<IEnumerable<TicketResponseDTO>> GetMyTickets(Guid userId)
    {
        var tickets = (await _ticketRepository.GetAll())
            .Where(t => t.UserId == userId && t.Status != TicketStatus.Cancelled)
            .ToList();
        
        var responses = new List<TicketResponseDTO>();

        foreach (var ticket in tickets)
        {
            var eventObj = await _eventRepository.GetById(ticket.EventId);
            var ticketType = await _ticketTypeRepository.GetById(ticket.TicketTypeId);
            Payment? payment = ticket.PaymentId.HasValue
            ? await _paymentRepository.GetById(ticket.PaymentId.Value)
            : null;

            var response = new TicketResponseDTO
            {
                Id = ticket.Id,
                EventTitle = eventObj?.Title ?? "",
                TicketType = ticketType?.TypeName.ToString() ?? "",
                PricePerTicket = ticketType?.Price ?? 0,
                Quantity = ticket.BookedQuantity,
                BookedAt = ticket.BookedAt,
                Payment = payment != null ? new PaymentResponseDTO
                {
                    Id = payment.Id,
                    PaymentType = payment.PaymentType,
                    Amount = payment.Amount,
                    Status = payment.PaymentStatus,
                    TransactionId = payment.TransactionId
                } : null
            };

            if (eventObj?.EventType == EventType.Seatable)
            {
                response.SeatNumbers = (await _bookedSeatRepository.GetAll())
                    .Where(bs => bs.TicketId == ticket.Id &&
                                bs.BookedSeatStatus == BookedSeatStatus.Booked)
                    .Select(bs => bs.SeatNumber)
                    .ToList();
            }
            responses.Add(response);
        }

        return responses;
    }

    public async Task<TicketResponseDTO> GetTicketById(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        
        var eventObj = await _eventRepository.GetById(ticket.EventId);
        var ticketType = await _ticketTypeRepository.GetById(ticket.TicketTypeId);
        Payment? payment = ticket.PaymentId.HasValue 
        ? await _paymentRepository.GetById(ticket.PaymentId.Value) 
        : null;
        var response = new TicketResponseDTO
        {
            Id = ticket.Id,
            EventTitle = eventObj?.Title ?? "",
            TicketType = ticketType?.TypeName.ToString() ?? "",
            PricePerTicket = ticketType?.Price ?? 0,
            Quantity = ticket.BookedQuantity,
            BookedAt = ticket.BookedAt,
            Payment = payment != null ? new PaymentResponseDTO
            {
                Id = payment.Id,
                PaymentType = payment.PaymentType,
                Amount = payment.Amount,
                Status = payment.PaymentStatus,
                TransactionId = payment.TransactionId
            } : null
        };

        if (eventObj?.EventType == EventType.Seatable)
        {
            var bookedSeats = (await _bookedSeatRepository.GetAll())
                .Where(bs => bs.TicketId == ticket.Id && 
                             bs.BookedSeatStatus == BookedSeatStatus.Booked)
                .Select(bs => bs.SeatNumber)
                .ToList();
            
            response.SeatNumbers = bookedSeats;
        }

        return response;
    }
    public async Task<IEnumerable<TicketResponseDTO>> GetTicketsByEventId(Guid eventId, Guid requesterId)
    {
        var evt = await _eventRepository.GetById(eventId);
        var user = await _userRepository.GetById(requesterId);

        if (user == null) throw new Exception("User not found");
        if (evt == null || evt.IsDeleted) throw new Exception("Event not found");

        if (user.Role != UserRole.Admin && evt.ManagerId != requesterId)
            throw new UnauthorizedAccessException("Access denied");

        var tickets = (await _ticketRepository.GetAll())
            .Where(t => t.EventId == eventId)
            .ToList();

        var responses = new List<TicketResponseDTO>();

        foreach (var ticket in tickets)
        {
            var ticketType = await _ticketTypeRepository.GetById(ticket.TicketTypeId);
            Payment? payment = ticket.PaymentId.HasValue 
            ? await _paymentRepository.GetById(ticket.PaymentId.Value) 
            : null;
            var response = new TicketResponseDTO
            {
                Id = ticket.Id,
                EventTitle = evt.Title ?? "",
                TicketType = ticketType?.TypeName.ToString() ?? "",
                PricePerTicket = ticketType?.Price ?? 0,
                Quantity = ticket.BookedQuantity,
                BookedAt = ticket.BookedAt,
                Payment = payment != null ? new PaymentResponseDTO
                {
                    Id = payment.Id,
                    PaymentType = payment.PaymentType,
                    Amount = payment.Amount,
                    Status = payment.PaymentStatus,
                    TransactionId = payment.TransactionId
                } : null
            };

            if (evt.EventType == EventType.Seatable)
            {
                var bookedSeats = (await _bookedSeatRepository.GetAll())
                    .Where(bs => bs.TicketId == ticket.Id &&
                                 bs.BookedSeatStatus == BookedSeatStatus.Booked)
                    .Select(bs => bs.SeatNumber)
                    .ToList();

                response.SeatNumbers = bookedSeats;
            }

            responses.Add(response);
        }

        return responses;
    }
    public async Task<byte[]> ExportTicketAsPdf(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Unauthorized");

        var eventObj = await _eventRepository.GetById(ticket.EventId);
        var ticketType = await _ticketTypeRepository.GetById(ticket.TicketTypeId);
        var payment = ticket.PaymentId.HasValue 
            ? await _paymentRepository.GetById(ticket.PaymentId.Value) 
            : null;
        
        var content = new StringBuilder();
        content.AppendLine($"TICKET RECEIPT");
        content.AppendLine($"----------------------------------");
        content.AppendLine($"Event: {eventObj?.Title}");
        content.AppendLine($"Ticket Type: {ticketType?.TypeName}");
        content.AppendLine($"Quantity: {ticket.BookedQuantity}");
        content.AppendLine($"Total Price: {ticket.TotalPrice:C}");
        
        if (payment != null)
        {
            content.AppendLine($"Payment Method: {payment.PaymentType}");
            content.AppendLine($"Transaction ID: {payment.TransactionId}");
            content.AppendLine($"Payment Status: {payment.PaymentStatus.ToString()}");
        }
        
        content.AppendLine($"Booked At: {ticket.BookedAt:g}");
        
        if (eventObj?.EventType == EventType.Seatable)
        {
            var bookedSeats = (await _bookedSeatRepository.GetAll())
                .Where(bs => bs.TicketId == ticketId &&
                             bs.BookedSeatStatus == BookedSeatStatus.Booked)
                .Select(bs => bs.SeatNumber)
                .ToList();

            if (bookedSeats.Any())
            {
                content.AppendLine($"Seat Numbers: {string.Join(", ", bookedSeats)}");
            }
        }

        content.AppendLine($"----------------------------------");
        content.AppendLine($"Thank you for your booking!");

        return Encoding.UTF8.GetBytes(content.ToString());
    }
}