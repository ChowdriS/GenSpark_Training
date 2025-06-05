using System;

namespace AppointmentApi.Models.DTO;

public class CustomExceptionDTO
{
    public int errorNumber { get; set; }
    public string? errorMessage { get; set; }
}
