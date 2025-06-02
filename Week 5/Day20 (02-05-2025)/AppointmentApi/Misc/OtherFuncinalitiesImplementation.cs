using System;
using AppointmentApi.Context;
using AppointmentApi.Interface;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Misc;

public class OtherFuncinalitiesImplementation : IOtherContextFunctionities
    {
        private readonly ClinicContext _clinicContext;

        public OtherFuncinalitiesImplementation(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<ICollection<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string specilaity)
        {
            var result = await _clinicContext.GetDoctorsBySpeciality(specilaity);
            return result;
        }
    }
