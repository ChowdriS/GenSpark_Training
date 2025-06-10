using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IEventService
{
    public Task<PaginatedResultDTO<Event>> GetAllEvents(int pageNumber, int pageSize);
    public Task<EventResponseDTO> GetEventById(Guid id);
    public Task<PaginatedResultDTO<Event>> FilterEvents(string searchElement, DateTime? date, int pageNumber, int pageSize);
    public Task<PaginatedResultDTO<Event>> GetManagedEventsByUserId(Guid managerId, int pageNumber, int pageSize);
    public Task<EventResponseDTO> AddEvent(EventAddRequestDTO dto,Guid ManagerId);
    public Task<EventResponseDTO> UpdateEvent(Guid id, EventUpdateRequestDTO dto);
    public Task<EventResponseDTO> DeleteEvent(Guid id);
}
