using System;

namespace EventBookingApi.Model;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? UserEmail { get; set; }
    public User? User { get; set; }

    public Guid EventId { get; set; }
    public Event? Event { get; set; }

    public Guid TicketTypeId { get; set; }
    public TicketType? TicketType { get; set; }

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Booked";

    public bool IsDeleted { get; set; } = false;
}

