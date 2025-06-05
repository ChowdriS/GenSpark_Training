using System;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Service;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<string, Appointment> _appointmentRepository;
    private readonly IRepository<int, Doctor> _doctorRepository;
    private readonly IRepository<int, Patient> _patientRepository;

    public AppointmentService(IRepository<string, Appointment> appointmentRepository,
                              IRepository<int, Doctor> doctorRepository,
                              IRepository<int, Patient> patientRepository
                                )
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
    }

    public async Task<Appointment> AddAppointment(AppointmentRequestDTO dto)
    {
        try
        {
            var doctor = await _doctorRepository.GetById(dto.DoctorId);
            var patient = await _patientRepository.GetById(dto.PatientId);
            
            var dtoTimeUtc = dto.AppointmentDateTime.ToUniversalTime();
            dtoTimeUtc = new DateTime(dtoTimeUtc.Year, dtoTimeUtc.Month, dtoTimeUtc.Day, 
                                    dtoTimeUtc.Hour, dtoTimeUtc.Minute, dtoTimeUtc.Second, 
                                    0, DateTimeKind.Utc);

            bool hasConflict = false;
            try
            {
                var doctorAppointments = (await _appointmentRepository.GetAll())
                    .Where(a => a.DoctorId == dto.DoctorId)
                    .ToList();
                foreach (var app in doctorAppointments)
                {
                    var appTimeUtc = app.AppointmentDateTime.ToUniversalTime();
                    appTimeUtc = new DateTime(appTimeUtc.Year, appTimeUtc.Month, appTimeUtc.Day,
                                            appTimeUtc.Hour, appTimeUtc.Minute, appTimeUtc.Second,
                                            0, DateTimeKind.Utc);


                    if (appTimeUtc == dtoTimeUtc)
                    {
                        hasConflict = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {}
            if (hasConflict == true)
            {
                throw new Exception("Sorry, the doctor already has an appointment at that time");
            }
            

            var appointment = new Appointment
            {
                AppointmentNumber = $"{DateTime.UtcNow:yyyyMMddHHmmss}{doctor.Id}{patient.Id}",
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDateTime = dtoTimeUtc,
                Status = "active"
            };

            appointment = await _appointmentRepository.Add(appointment);
            return appointment;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Appointment> CancelAppointment(string appointmentId)
    {
        try
        {
            var appointment = await _appointmentRepository.GetById(appointmentId);
            if (appointment.Status == "Cancelled")
            {
                throw new Exception("The Appointment is already Cancelled");
            }
            appointment.Status = "Cancelled";
            appointment = await _appointmentRepository.Update(appointmentId, appointment);
            return appointment;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ICollection<Appointment>> GetAll()
    {
        var result = await _appointmentRepository.GetAll();
        return result.ToList();
    }
}
