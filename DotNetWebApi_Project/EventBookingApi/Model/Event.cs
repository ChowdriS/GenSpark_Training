using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model;

public class Event
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string? Title { get; set; }

    public string? Description { get; set; }
    public DateTime EventDate { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Active;

    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid? ManagerId { get; set; }
    public User? Manager { get; set; }

    public ICollection<TicketType>? TicketTypes { get; set; }
    public ICollection<Ticket>? Tickets { get; set; }
}