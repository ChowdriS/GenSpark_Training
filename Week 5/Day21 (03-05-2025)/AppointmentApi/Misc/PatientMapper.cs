using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Misc;

public class PatientMapper
{
    public Patient? MapPatientAddRequestPatient(PatientAddRequestDTO addRequestDto)
        {
            Patient Patient = new();
            Patient.Name = addRequestDto.Name;
            Patient.Age = addRequestDto.Age;
            Patient.Phone = addRequestDto.Phone;
            Patient.Email = addRequestDto.Email;
            return Patient;
        }
}
