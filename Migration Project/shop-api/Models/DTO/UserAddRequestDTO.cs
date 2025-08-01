using System;

namespace shop_api.Models.DTO;

public class UserAddRequestDTO
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
