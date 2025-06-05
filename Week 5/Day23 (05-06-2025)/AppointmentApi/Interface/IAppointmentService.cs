using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Interface;

public interface IAppointmentService
{
    public Task<ICollection<Appointment>> GetAll();
    public Task<Appointment> AddAppointment(AppointmentRequestDTO dto);
    public Task<Appointment> CancelAppointment(string appointmentId);
}
