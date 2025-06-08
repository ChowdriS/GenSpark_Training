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

    public TicketService(
        IRepository<Guid, Ticket> ticketRepository,
        IRepository<Guid, Event> eventRepository,
        IRepository<Guid, TicketType> ticketTypeRepository,
        IRepository<Guid, User> userRepository)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _userRepository = userRepository;
    }

    public async Task<TicketResponseDTO> BookTicket(TicketBookRequestDTO dto, Guid userId)
    {
        var ticketType = await _ticketTypeRepository.GetById(dto.TicketTypeId);
        if (ticketType == null || ticketType.TotalQuantity - ticketType.BookedQuantity <= 0)
            throw new Exception("Ticket type not available!");
        var remaining_ticket = ticketType.TotalQuantity - ticketType.BookedQuantity;
        if (remaining_ticket < dto.Quantity)
            throw new Exception("The Request Count is Hhigher than Available!");
        ticketType.BookedQuantity += dto.Quantity;
        await _ticketTypeRepository.Update(ticketType.Id, ticketType);

        var ticket = new Ticket
        {
            UserId = userId,
            EventId = ticketType.EventId,
            TicketTypeId = ticketType.Id,
            BookedQuantity = dto.Quantity,
            TotalPrice = dto.Quantity * ticketType.Price
        };

        var created = await _ticketRepository.Add(ticket);
        var eventBooked = await _eventRepository.GetById(dto.EventId);
        return new TicketResponseDTO
        {
            Id = created.Id,
            EventTitle = eventBooked.Title ?? "",
            TicketType = ticketType.TypeName.ToString(),
            PricePerTicket = ticketType.Price,
            Quantity = ticket.BookedQuantity,
            BookedAt = created.BookedAt
        };
    }

    public async Task<IEnumerable<TicketResponseDTO>> GetMyTickets(Guid userId)
    {
        var tickets = await _ticketRepository.GetAll();

        var ticketResponses = new List<TicketResponseDTO>();

        foreach (var ticket in tickets.Where(t => t.UserId == userId))
        {
            var bookedEvent = await _eventRepository.GetById(ticket.EventId);

            var ticketResponse = new TicketResponseDTO
            {
                Id = ticket.Id,
                EventTitle = bookedEvent.Title ?? "",
                TicketType = ticket?.TicketType?.TypeName.ToString() ?? "",
                PricePerTicket = ticket?.TicketType?.Price ?? 0,
                BookedAt = ticket?.BookedAt
            };

            ticketResponses.Add(ticketResponse);
        }

        return ticketResponses;
    }


    public async Task<TicketResponseDTO> GetTicketById(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Access denied");
        var bookedEvent = await _eventRepository.GetById(ticket.EventId);
        return new TicketResponseDTO
        {
            Id = ticket.Id,
            EventTitle = bookedEvent.Title ?? "",
            TicketType = ticket?.TicketType?.TypeName.ToString() ?? "",
            PricePerTicket = ticket?.TicketType?.Price ?? 0,
            BookedAt = ticket?.BookedAt
        };
    }

    public async Task<bool> CancelTicket(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Cannot cancel this ticket");
        ticket.Status = TicketStatus.Cancelled;
        await _ticketRepository.Update(ticketId, ticket);
        return true;
    }

    public async Task<byte[]> ExportTicketAsPdf(Guid ticketId, Guid userId)
    {
        var ticket = await _ticketRepository.GetById(ticketId);
        if (ticket == null || ticket.UserId != userId)
            throw new UnauthorizedAccessException("Unauthorized");

        // Simulate PDF export
        var pdfData = Encoding.UTF8.GetBytes($"Ticket ID: {ticket.Id}\nEvent ID: {ticket.EventId}");
        return pdfData;
    }

    public async Task<IEnumerable<TicketResponseDTO>> GetTicketsByEventId(Guid eventId, Guid requesterId)
    {
        var evt = await _eventRepository.GetById(eventId);
        var user = await _userRepository.GetById(requesterId);
        if (user == null)
            throw new Exception("User not Found");
        if (evt == null)
            throw new Exception("Event not found");

        if (user.Role.ToString() != "Admin" && evt.ManagerId != requesterId)
            throw new UnauthorizedAccessException("Access denied");

        var tickets = await _ticketRepository.GetAll();
        return tickets.Where(t => t.EventId == eventId)
                      .Select(t => new TicketResponseDTO
                      {
                          Id = t.Id,
                          EventTitle = evt.Title ?? "",
                          TicketType = t?.TicketType?.TypeName.ToString() ?? "",
                          PricePerTicket = t?.TicketType?.Price ?? 0,
                          BookedAt = t?.BookedAt
                      });
    }
}
