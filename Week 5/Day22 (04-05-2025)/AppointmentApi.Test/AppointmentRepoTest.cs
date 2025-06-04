using AppointmentApi.Context;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using AppointmentApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Test;
public class AppointmentRepositoryTests
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ClinicContext(options);

        _context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. A", Email = "a@ex.com", YearsOfExperience = 10 });
        _context.Patients.Add(new Patient { Id = 1, Name = "abc", Age = 30, Email = "abc@ex.com" });
        _context.SaveChanges();
    }

    [Test]
    public async Task AddAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentNumber = "APT001",
            PatientId = 1,
            DoctorId = 1,
            AppointmentDateTime = DateTime.Now,
            Status = "Active"
        };
        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        // Act
        var result = await repo.Add(appointment);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.AppointmentNumber, Is.EqualTo("APT001"));
    }

    [Test]
    public async Task GetAppointmentByIdTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentNumber = "APT002",
            PatientId = 1,
            DoctorId = 1,
            AppointmentDateTime = DateTime.Now,
            Status = "Active"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        // Act
        var result = await repo.GetById("APT002");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Status, Is.EqualTo("Active"));
    }

    [Test]
    public async Task GetAllAppointmentsTest()
    {
        // Arrange
        _context.Appointments.AddRange(
            new Appointment { AppointmentNumber = "APT003", PatientId = 1, DoctorId = 1, AppointmentDateTime = DateTime.Now, Status = "Pending" },
            new Appointment { AppointmentNumber = "APT004", PatientId = 1, DoctorId = 1, AppointmentDateTime = DateTime.Now, Status = "Done" }
        );
        await _context.SaveChangesAsync();

        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        // Act
        var result = await repo.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentNumber = "APT005",
            PatientId = 1,
            DoctorId = 1,
            AppointmentDateTime = DateTime.Now,
            Status = "Active"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        var updated = new Appointment
        {
            AppointmentNumber = "APT005",
            PatientId = 1,
            DoctorId = 1,
            AppointmentDateTime = DateTime.Now.AddDays(1),
            Status = "Rescheduled"
        };

        // Act
        var result = await repo.Update("APT005", updated);

        // Assert
        Assert.That(result.Status, Is.EqualTo("Rescheduled"));
    }

    [Test]
    public async Task DeleteAppointmentTest()
    {
        // Arrange
        var appointment = new Appointment
        {
            AppointmentNumber = "APT006",
            PatientId = 1,
            DoctorId = 1,
            AppointmentDateTime = DateTime.Now,
            Status = "Active"
        };
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        // Act
        var result = await repo.Delete("APT006");

        // Assert
        Assert.That(result.AppointmentNumber, Is.EqualTo("APT006"));
        Assert.That(await _context.Appointments.FindAsync("APT006"), Is.Null);
    }

    [Test]
    public void GetById_NotFound_ThrowsException()
    {
        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetById("INVALID_ID"));
        Assert.That(ex.Message, Is.EqualTo("No appointment with the given ID"));
    }

    [Test]
    public void GetAll_NoAppointments_ThrowsException()
    {
        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetAll());
        Assert.That(ex.Message, Is.EqualTo("No appointments in the database"));
    }

    [Test]
    public void Delete_NotFound_ThrowsException()
    {
        IRepository<string, Appointment> repo = new AppointmentRepository(_context);

        var ex = Assert.ThrowsAsync<Exception>(async () => await repo.Delete("UNKNOWN"));
        Assert.That(ex.Message, Is.EqualTo("No appointment with the given ID"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
