using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Misc;

public class SpecialityMapper
    {
        public Speciality? MapSpecialityAddRequestDoctor(SpecialityAddRequestDTO addRequestDto)
        {
            Speciality speciality = new();
            speciality.Name = addRequestDto.Name;
            return speciality;
        }

        public DoctorSpeciality MapDoctorSpecility(int doctorId, int specialityId)
        {
            DoctorSpeciality doctorSpeciality = new();
            doctorSpeciality.DoctorId = doctorId;
            doctorSpeciality.SpecialityId = specialityId;
            return doctorSpeciality;
        }
    }