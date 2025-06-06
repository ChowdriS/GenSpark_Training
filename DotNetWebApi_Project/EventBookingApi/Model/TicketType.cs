using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model;

public class TicketType
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid EventId { get; set; }
    public Event? Event { get; set; }

    [Required]
    public string? TypeName { get; set; } // "Regular", "VIP", etc.

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int TotalQuantity { get; set; }

    public int BookedQuantity { get; set; } = 0;

    public string Status { get; set; } = "Available"; 

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

