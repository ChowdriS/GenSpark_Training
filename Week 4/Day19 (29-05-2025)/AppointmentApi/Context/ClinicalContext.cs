using System;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Context;

public class ClinicContext : DbContext
{
    public ClinicContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }

    public DbSet<DoctorsBySpecialityResponseDto> DoctorsBySpeciality{ get; set; }

    public async Task<List<DoctorsBySpecialityResponseDto>> GetDoctorsBySpeciality(string speciality)
    {
        return await this.Set<DoctorsBySpecialityResponseDto>()
                    .FromSqlInterpolated($"select * from proc_GetDoctorsBySpeciality({speciality})")
                    .ToListAsync();
    }

}
