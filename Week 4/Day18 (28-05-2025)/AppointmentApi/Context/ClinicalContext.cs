using System;
using AppointmentApi.Models;
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
    
}
