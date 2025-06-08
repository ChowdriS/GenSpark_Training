using System;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IEventService
{
    public Task<IEnumerable<EventResponseDTO>> GetAllEvents();
    public Task<EventResponseDTO> GetEventById(Guid id);
    public Task<IEnumerable<EventResponseDTO>> FilterEvents(string city, DateTime? date);
    public Task<IEnumerable<EventResponseDTO>> GetManagedEventsByUserId(Guid managerId);
    public Task<EventResponseDTO> AddEvent(EventAddRequestDTO dto);
    public Task<EventResponseDTO> UpdateEvent(Guid id, EventUpdateRequestDTO dto);
    public Task<EventResponseDTO> DeleteEvent(Guid id);
}
