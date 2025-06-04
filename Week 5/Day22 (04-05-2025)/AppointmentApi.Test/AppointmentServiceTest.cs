using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;
using AppointmentApi.Service;
using Moq;
using NUnit.Framework;

namespace AppointmentApi.Test;
public class AppointmentServiceTest
{
    private Mock<IRepository<string, Appointment>> _appointmentRepoMock;
    private Mock<IRepository<int, Doctor>> _doctorRepoMock;
    private Mock<IRepository<int, Patient>> _patientRepoMock;
    private AppointmentService _appointmentService;

    [SetUp]
    public void SetUp()
    {
        _appointmentRepoMock = new Mock<IRepository<string, Appointment>>();
        _doctorRepoMock = new Mock<IRepository<int, Doctor>>();
        _patientRepoMock = new Mock<IRepository<int, Patient>>();

        _appointmentService = new AppointmentService(
            _appointmentRepoMock.Object,
            _doctorRepoMock.Object,
            _patientRepoMock.Object
        );
    }

    [Test]
    public async Task AddAppointment_ShouldSucceed_WhenNoTimeConflict()
    {
        // Arrange
        var dto = new AppointmentRequestDTO
        {
            DoctorId = 1,
            PatientId = 1,
            AppointmentDateTime = DateTime.UtcNow.AddHours(2)
        };

        var doctor = new Doctor { Id = 1 };
        var patient = new Patient { Id = 1 };
        _doctorRepoMock.Setup(r => r.GetById(dto.DoctorId)).ReturnsAsync(doctor);
        _patientRepoMock.Setup(r => r.GetById(dto.PatientId)).ReturnsAsync(patient);
        _appointmentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Appointment>());
        _appointmentRepoMock.Setup(r => r.Add(It.IsAny<Appointment>()))
                            .ReturnsAsync((Appointment a) => a);

        // Act
        var result = await _appointmentService.AddAppointment(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo("active"));
        Assert.That(result.AppointmentNumber, Does.Contain("1"));
    }

    [Test]
    public void AddAppointment_ShouldFail_WhenTimeConflict()
    {
        // Arrange
        var conflictTime = DateTime.UtcNow.AddHours(1);
        var dto = new AppointmentRequestDTO
        {
            DoctorId = 1,
            PatientId = 1,
            AppointmentDateTime = conflictTime
        };

        var doctor = new Doctor { Id = 1 };
        var patient = new Patient { Id = 1 };

        _doctorRepoMock.Setup(r => r.GetById(dto.DoctorId)).ReturnsAsync(doctor);
        _patientRepoMock.Setup(r => r.GetById(dto.PatientId)).ReturnsAsync(patient);
        _appointmentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Appointment>
        {
            new Appointment
            {
                DoctorId = 1,
                AppointmentDateTime = conflictTime,
                Status = "active"
            }
        });

        // Act + Assert
        var ex = Assert.ThrowsAsync<Exception>(() => _appointmentService.AddAppointment(dto));
        Assert.That(ex.Message, Is.EqualTo("Sorry, the doctor already has an appointment at that time"));
    }

    [Test]
    public async Task CancelAppointment_ShouldSucceed()
    {
        var appointmentId = "appt1";
        var appointment = new Appointment
        {
            AppointmentNumber = appointmentId,
            Status = "active"
        };

        _appointmentRepoMock.Setup(r => r.GetById(appointmentId)).ReturnsAsync(appointment);
        _appointmentRepoMock.Setup(r => r.Update(appointmentId, It.IsAny<Appointment>()))
                            .ReturnsAsync(appointment);
        // System.Console.WriteLine(appointment.Status);
        var result = await _appointmentService.CancelAppointment(appointmentId);
        // System.Console.WriteLine(appointment.Status);

        Assert.That(result.Status, Is.EqualTo("Cancelled"));
    }

    [Test]
    public async Task GetAll_ShouldReturnAppointments()
    {
        var appointments = new List<Appointment>
        {
            new Appointment { AppointmentNumber = "1", Status = "active" },
            new Appointment { AppointmentNumber = "2", Status = "active" }
        };

        _appointmentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(appointments);

        var result = await _appointmentService.GetAll();

        Assert.That(result.Count, Is.EqualTo(2));
    }
}
