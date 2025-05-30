using System;
using AppointmentApi.Models;

namespace AppointmentApi.Interface;

public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<Patient?> GetPatientById(int id);
        Task<Patient> CreatePatient(Patient Patient);
        Task<Patient?> UpdatePatient(int id, Patient Patient);
        Task<Patient> DeletePatient(int id);
    }
