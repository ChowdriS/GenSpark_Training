using System;

namespace AppointmentApi.Models.DTO;

public class PatientAddRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
        
}
