using System;
using AppointmentApi.Context;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Test;

public class SpecialityRepoTest
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ClinicContext(options);
    }

    [Test]
    public async Task AddSpecialityTest()
    {
        // Arrange
        var speciality = new Speciality { Name = "Cardiology" };
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act
        var result = await repo.Add(speciality);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Cardiology"));
    }

    [Test]
    public async Task GetSpecialityByIdTest()
    {
        // Arrange
        var speciality = new Speciality { Name = "Neurology" };
        _context.Specialities.Add(speciality);
        await _context.SaveChangesAsync();
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act
        var result = await repo.GetById(speciality.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo("Neurology"));
    }

    [Test]
    public async Task GetAllSpecialitiesTest()
    {
        // Arrange
        _context.Specialities.AddRange(
            new Speciality { Name = "Ortho" },
            new Speciality { Name = "Dental" }
        );
        await _context.SaveChangesAsync();
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act
        var result = await repo.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateSpecialityTest()
    {
        // Arrange
        var speciality = new Speciality { Name = "Skin" };
        _context.Specialities.Add(speciality);
        await _context.SaveChangesAsync();
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        var updatedSpeciality = new Speciality { Id = speciality.Id, Name = "Dermatology" };

        // Act
        var result = await repo.Update(speciality.Id, updatedSpeciality);

        // Assert
        Assert.That(result.Name, Is.EqualTo("Dermatology"));
    }

    [Test]
    public async Task DeleteSpecialityTest()
    {
        // Arrange
        var speciality = new Speciality { Name = "Eye" };
        _context.Specialities.Add(speciality);
        await _context.SaveChangesAsync();
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act
        var result = await repo.Delete(speciality.Id);

        // Assert
        Assert.That(result.Id, Is.EqualTo(speciality.Id));
        Assert.That(await _context.Specialities.FindAsync(speciality.Id), Is.Null);
    }

    [Test]
    public void GetById_NotFound_ThrowsException()
    {
        // Arrange
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetById(999));
        Assert.That(ex.Message, Is.EqualTo("No speciality with the given ID"));
    }

    [Test]
    public void GetAll_NoSpecialities_ThrowsException()
    {
        // Arrange
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetAll());
        Assert.That(ex.Message, Is.EqualTo("No specialities in the database"));
    }

    [Test]
    public void Delete_NotFound_ThrowsException()
    {
        // Arrange
        IRepository<int, Speciality> repo = new SpecialityRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.Delete(999));
        Assert.That(ex.Message, Is.EqualTo("No speciality with the given ID"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}