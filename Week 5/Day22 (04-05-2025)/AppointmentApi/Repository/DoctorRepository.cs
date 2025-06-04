using System;
using AppointmentApi.Context;
using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Repository;

public class DoctorRepository : Repository<int, Doctor>
{
    public DoctorRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }

    public override async Task<Doctor> GetById(int key)
    {
        var doctor = await _clinicContext.Doctors.SingleOrDefaultAsync(p => p.Id == key);

        return doctor ?? throw new Exception("No doctor with the given ID");
    }

    public override async Task<IEnumerable<Doctor>> GetAll()
    {
        var doctors = _clinicContext.Doctors;
        if (doctors.Count() == 0)
            throw new Exception("No doctors in the database");
        return await doctors.ToListAsync();
    }
    
}
