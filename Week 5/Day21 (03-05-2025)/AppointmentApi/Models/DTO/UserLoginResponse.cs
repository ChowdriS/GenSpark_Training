using System;

namespace AppointmentApi.Models.DTO;

public class UserLoginResponse
{
    public string Username { get; set; } = string.Empty;
    public string? Token { get; set; }
}
