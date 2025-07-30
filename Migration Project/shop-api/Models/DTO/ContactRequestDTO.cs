using System;

namespace shop_api.Models.DTO;

public class ContactRequestDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Content { get; set; }
}
