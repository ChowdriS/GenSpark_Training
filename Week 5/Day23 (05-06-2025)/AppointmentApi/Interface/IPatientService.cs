using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Interface;

public interface IPatientService
    {
        public Task<Patient> CreatePatient(PatientAddRequestDTO Patient);
    }
