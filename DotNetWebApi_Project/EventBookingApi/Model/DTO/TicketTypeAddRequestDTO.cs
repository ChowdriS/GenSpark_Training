using System;
using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Model.DTO;

public class TicketTypeAddRequestDTO
{
    [Required]
    public TicketTypeEnum TypeName { get; set; } = TicketTypeEnum.Regular;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int TotalQuantity { get; set; }

    public string? Description { get; set; }
}
