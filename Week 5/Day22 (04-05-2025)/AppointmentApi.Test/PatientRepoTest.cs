using System;
using AppointmentApi.Context;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Test;

public class PatinetRepositoryTests
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Isolated db per test
            .Options;

        _context = new ClinicContext(options);
    }

    [Test]
    public async Task AddPatientTest()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Test Patient",
            Age = 30,
            Email = "patient@example.com",
            Phone = "1234567890"
        };

        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act
        var result = await repo.Add(patient);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Test Patient"));
    }

    [Test]
    public async Task GetPatientById_ValidId_ReturnsPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "John Doe",
            Age = 25,
            Email = "john@example.com",
            Phone = "9999999999"
        };
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act
        var result = await repo.GetById(patient.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Email, Is.EqualTo("john@example.com"));
    }

    [Test]
    public void GetPatientById_InvalidId_ThrowsException()
    {
        // Arrange
        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetById(999));
        Assert.That(ex.Message, Is.EqualTo("No patient with teh given ID"));
    }

    [Test]
    public async Task GetAllPatients_ReturnsPatients()
    {
        // Arrange
        _context.Patients.AddRange(
            new Patient { Name = "A", Age = 20, Email = "a@gmail.com", Phone = "123" },
            new Patient { Name = "B", Age = 22, Email = "b@gmail.com", Phone = "456" }
        );
        await _context.SaveChangesAsync();

        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act
        var result = await repo.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetAll_NoPatients_ThrowsException()
    {
        // Arrange
        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetAll());
        Assert.That(ex.Message, Is.EqualTo("No Patients in the database"));
    }

    [Test]
    public async Task UpdatePatientTest()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "Old Name",
            Age = 40,
            Email = "old@example.com",
            Phone = "1111111111"
        };
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        IRepository<int, Patient> repo = new PatinetRepository(_context);

        var updatedPatient = new Patient
        {
            Id = patient.Id,
            Name = "New Name",
            Age = 45,
            Email = "new@example.com",
            Phone = "2222222222"
        };

        // Act
        var result = await repo.Update(patient.Id, updatedPatient);

        // Assert
        Assert.That(result.Name, Is.EqualTo("New Name"));
        Assert.That(result.Age, Is.EqualTo(45));
        Assert.That(result.Email, Is.EqualTo("new@example.com"));
    }
    [Test]
    public async Task DeletePatientTest()
    {
        // Arrange
        var patient = new Patient
        {
            Name = "To Be Deleted",
            Age = 50,
            Email = "delete@example.com",
            Phone = "3333333333"
        };
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        IRepository<int, Patient> repo = new PatinetRepository(_context);

        // Act
        var result = await repo.Delete(patient.Id);

        // Assert
        Assert.That(result.Id, Is.EqualTo(patient.Id));
        Assert.That(await _context.Patients.FindAsync(patient.Id), Is.Null);
    }


    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}