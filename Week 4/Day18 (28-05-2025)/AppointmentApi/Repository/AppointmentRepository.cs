using System;
using AppointmentApi.Context;
using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Repository;

public class AppointmentRepository : Repository<string, Appointment>
{
    public AppointmentRepository(ClinicContext clinicContext) : base(clinicContext) { }

    public override async Task<Appointment> GetById(string key)
    {
        var appointment = await _clinicContext.Appointments.SingleOrDefaultAsync(a => a.AppointmentNumber == key);
        return appointment ?? throw new Exception("No appointment with the given ID");
    }

    public override async Task<IEnumerable<Appointment>> GetAll()
    {
        var appointments = _clinicContext.Appointments;
        if (!appointments.Any())
            throw new Exception("No appointments in the database");

        return await appointments.ToListAsync();
    }
}
