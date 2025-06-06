using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model;

public class User
{
    [Required, EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? PasswordHash { get; set; }

    [Required]
    public string? Role { get; set; } 

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Event>? ManagedEvents { get; set; }
    public ICollection<Ticket>? Tickets { get; set; }
}
