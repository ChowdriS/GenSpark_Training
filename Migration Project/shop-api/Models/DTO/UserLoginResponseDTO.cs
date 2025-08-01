using System;

namespace shop_api.Models.DTO;

public class UserLoginResponseDTO
{
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
