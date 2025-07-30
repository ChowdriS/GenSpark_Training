using System;

namespace shop_api.Models.DTO;

public class OrderUpdateRequestDTO
{
    public string? Status { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerEmail { get; set; }
    public string? CustomerAddress { get; set; }
}
