using System;
using AppointmentApi.Misc;

namespace AppointmentApi.Models.DTO;

public class DoctorAddRequestDto
{
    [NameValidation]
    public string Name { get; set; } = string.Empty;
    public ICollection<SpecialityAddRequestDTO>? Specialities { get; set; }

    public string Email { get; set; } = string.Empty;
    public float YearsOfExperience { get; set; }
    public string Password { get; set; } = string.Empty;
}
