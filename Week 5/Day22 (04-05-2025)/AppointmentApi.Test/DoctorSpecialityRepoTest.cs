using AppointmentApi.Context;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Tests;
public class DoctorSpecialityRepositoryTests
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ClinicContext(options);

        // Seed Doctors and Specialities for FK
        _context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. A", Email = "a@gmail.com", YearsOfExperience = 5 });
        _context.Specialities.Add(new Speciality { Id = 1, Name = "Cardiology" });
        _context.SaveChanges();
    }

    [Test]
    public async Task AddDoctorSpecialityTest()
    {
        var repo = new DoctorSpecialityRepository(_context);
        var ds = new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 };

        var result = await repo.Add(ds);

        Assert.That(result.SerialNumber, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetDoctorSpecialityByIdTest()
    {
        var ds = new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 };
        _context.DoctorSpecialities.Add(ds);
        await _context.SaveChangesAsync();

        var repo = new DoctorSpecialityRepository(_context);
        var result = await repo.GetById(ds.SerialNumber);

        Assert.IsNotNull(result);
        Assert.That(result.SerialNumber, Is.EqualTo(ds.SerialNumber));
    }

    [Test]
    public async Task GetAllDoctorSpecialitiesTest()
    {
        _context.DoctorSpecialities.AddRange(
            new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 },
            new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 }
        );
        await _context.SaveChangesAsync();

        var repo = new DoctorSpecialityRepository(_context);
        var result = await repo.GetAll();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateDoctorSpecialityTest()
    {
        var ds = new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 };
        _context.DoctorSpecialities.Add(ds);
        await _context.SaveChangesAsync();

        var repo = new DoctorSpecialityRepository(_context);
        var updated = new DoctorSpeciality
        {
            SerialNumber = ds.SerialNumber,
            DoctorId = 1,
            SpecialityId = 1 // Assume change if multiple specialities
        };

        var result = await repo.Update(ds.SerialNumber, updated);

        Assert.That(result.SerialNumber, Is.EqualTo(ds.SerialNumber));
    }

    [Test]
    public async Task DeleteDoctorSpecialityTest()
    {
        var ds = new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 };
        _context.DoctorSpecialities.Add(ds);
        await _context.SaveChangesAsync();

        var repo = new DoctorSpecialityRepository(_context);
        var result = await repo.Delete(ds.SerialNumber);

        Assert.That(result.SerialNumber, Is.EqualTo(ds.SerialNumber));
        Assert.That(await _context.DoctorSpecialities.FindAsync(ds.SerialNumber), Is.Null);
    }

    [Test]
    public void GetById_NotFound_ThrowsException()
    {
        var repo = new DoctorSpecialityRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetById(999));
        Assert.That(ex.Message, Is.EqualTo("No doctor-speciality mapping with the given ID"));
    }

    [Test]
    public void GetAll_NoMappings_ThrowsException()
    {
        var repo = new DoctorSpecialityRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetAll());
        Assert.That(ex.Message, Is.EqualTo("No doctor-speciality mappings in the database"));
    }

    [Test]
    public void Delete_NotFound_ThrowsException()
    {
        var repo = new DoctorSpecialityRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.Delete(999));
        Assert.That(ex.Message, Is.EqualTo("No doctor-speciality mapping with the given ID"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
