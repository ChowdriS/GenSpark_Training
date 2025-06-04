using System;

namespace AppointmentApi.Models.DTO;

public class AppointmentRequestDTO
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
}
