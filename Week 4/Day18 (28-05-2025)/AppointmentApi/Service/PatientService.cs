using System;
using System.Diagnostics;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Repository;

namespace AppointmentApi.Service;

public class PatientService : IPatientService
{
    private readonly IRepository<int,Patient> _patientRepository;

    public PatientService(IRepository<int,Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    async Task<Patient> IPatientService.CreatePatient(Patient Patient)
    {
        return await _patientRepository.Add(Patient);
    }

    async Task<Patient> IPatientService.DeletePatient(int id)
    {
        return await _patientRepository.Delete(id);
    }

    async Task<IEnumerable<Patient>> IPatientService.GetAllPatients()
    {
        return await _patientRepository.GetAll();
    }

    async Task<Patient?> IPatientService.GetPatientById(int id)
    {
        return await _patientRepository.GetById(id);
    }

    async Task<Patient?> IPatientService.UpdatePatient(int id, Patient Patient)
    {
        return await _patientRepository.Update(id, Patient);
    }

}
