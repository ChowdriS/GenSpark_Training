using System;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Interface;

public interface IOtherContextFunctionities
{
    public Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity);
}