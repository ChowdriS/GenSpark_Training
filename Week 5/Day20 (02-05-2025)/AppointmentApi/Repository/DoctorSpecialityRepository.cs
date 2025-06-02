using System;
using AppointmentApi.Context;
using AppointmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Repository;

public class DoctorSpecialityRepository : Repository<int, DoctorSpeciality>
{
    public DoctorSpecialityRepository(ClinicContext clinicContext) : base(clinicContext) { }

    public override async Task<DoctorSpeciality> GetById(int key)
    {
        var ds = await _clinicContext.DoctorSpecialities.SingleOrDefaultAsync(d => d.SerialNumber == key);
        return ds ?? throw new Exception("No doctor-speciality mapping with the given ID");
    }

    public override async Task<IEnumerable<DoctorSpeciality>> GetAll()
    {
        var dsList = _clinicContext.DoctorSpecialities;
        if (!dsList.Any())
            throw new Exception("No doctor-speciality mappings in the database");

        return await dsList.ToListAsync();
    }
}