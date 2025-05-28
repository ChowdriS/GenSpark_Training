using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Interface;

public interface IDoctorService
{
    public Task<Doctor> GetDoctorByName(string name);
    public Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality);
    public Task<Doctor> AddDoctor(DoctorAddRequestDto doctor);
}
