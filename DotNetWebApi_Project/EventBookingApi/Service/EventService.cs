using System;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;

public class EventService : IEventService
{
    private readonly IRepository<Guid, Event> _eventRepository;
    private readonly IRepository<Guid, TicketType> _ticketTypeRepository;
    private readonly IRepository<Guid, User> _userRepository;

    public EventService(IRepository<Guid, Event> eventRepository,
                        IRepository<Guid, TicketType> ticketTypeRepository,
                        IRepository<Guid, User> userRepository)
    {
        _eventRepository = eventRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<EventResponseDTO>> GetAllEvents()
    {
        var events = await _eventRepository.GetAll();
        return events
            .Where(e => !e.IsDeleted)
            .Select(e => MapToResponseDTO(e));
    }

    public async Task<EventResponseDTO> GetEventById(Guid id)
    {
        var ev = await _eventRepository.GetById(id);
        if (ev == null || ev.IsDeleted)
            throw new Exception("Event not found");

        return MapToResponseDTO(ev);
    }

    public async Task<IEnumerable<EventResponseDTO>> FilterEvents(string city, DateTime? date)
    {
        var events = await _eventRepository.GetAll();
        return events
            .Where(e =>
                !e.IsDeleted &&
                (string.IsNullOrEmpty(city) || e.Description?.Contains(city, StringComparison.OrdinalIgnoreCase) == true) &&
                (!date.HasValue || e.EventDate.Date == date.Value.Date))
            .Select(e => MapToResponseDTO(e));
    }

    public async Task<IEnumerable<EventResponseDTO>> GetManagedEventsByUserId(Guid managerId)
    {
        var events = await _eventRepository.GetAll();
        return events
            .Where(e => !e.IsDeleted && e.ManagerId == managerId)
            .Select(e => MapToResponseDTO(e));
    }

    public async Task<EventResponseDTO> AddEvent(EventAddRequestDTO dto)
    {
        var manager = await _userRepository.GetById(dto.ManagerId);

        var newEvent = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            EventType = dto.EventType,
            EventDate = dto.EventDate,
            ManagerId = manager?.Id
        };

        newEvent = await _eventRepository.Add(newEvent);

        if (dto.TicketTypes != null && dto.TicketTypes.Any())
        {
            foreach (var ticketDto in dto.TicketTypes)
            {
                var ticketType = new TicketType
                {
                    EventId = newEvent.Id,
                    TypeName = ticketDto.TypeName,
                    Price = ticketDto.Price,
                    TotalQuantity = ticketDto.TotalQuantity,
                    Description = ticketDto.Description
                };
                await _ticketTypeRepository.Add(ticketType);
            }
        }

        return MapToResponseDTO(newEvent);
    }

    public async Task<EventResponseDTO> UpdateEvent(Guid id, EventUpdateRequestDTO dto)
    {
        var existingEvent = await _eventRepository.GetById(id);
        existingEvent.Title = dto.Title ?? existingEvent.Title;
        existingEvent.Description = dto.Description ?? existingEvent.Description;
        existingEvent.EventDate = dto.EventDate ?? existingEvent.EventDate;
        existingEvent.EventType = dto.EventType ?? existingEvent.EventType; 
        existingEvent.UpdatedAt = DateTime.UtcNow;
        existingEvent.EventStatus = dto.EventStatus ?? existingEvent.EventStatus;
        var updatedEvent = await _eventRepository.Update(id, existingEvent);
        return MapToResponseDTO(updatedEvent);
    }

    public async Task<EventResponseDTO> DeleteEvent(Guid id)
    {
        var existingEvent = await _eventRepository.GetById(id);
        existingEvent.IsDeleted = true;
        existingEvent.UpdatedAt = DateTime.UtcNow;
        existingEvent.EventStatus = EventStatus.Cancelled;
        await _eventRepository.Update(id, existingEvent);
        return MapToResponseDTO(existingEvent);
    }

    private EventResponseDTO MapToResponseDTO(Event ev) => new()
    {
        Id = ev.Id,
        Title = ev.Title,
        Description = ev.Description,
        EventDate = ev.EventDate,
        EventStatus = ev.EventStatus.ToString(),
        EventType = ev.EventType.ToString() 
    };
}
