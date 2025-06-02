using System;
using AppointmentApi.Context;
using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Repository;

public class SpecialityRepository : Repository<int, Speciality>
{
    public SpecialityRepository(ClinicContext clinicContext) : base(clinicContext) { }

    public override async Task<Speciality> GetById(int key)
    {
        var speciality = await _clinicContext.Specialities.SingleOrDefaultAsync(s => s.Id == key);
        return speciality ?? throw new Exception("No speciality with the given ID");
    }

    public override async Task<IEnumerable<Speciality>> GetAll()
    {
        var specialities = _clinicContext.Specialities;
        if (!specialities.Any())
            throw new Exception("No specialities in the database");

        return await specialities.ToListAsync();
    }
}

