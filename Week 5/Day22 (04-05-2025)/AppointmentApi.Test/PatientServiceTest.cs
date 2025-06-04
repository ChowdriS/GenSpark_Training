using AppointmentApi.Context;
using AppointmentApi.Interface;
using AppointmentApi.Misc;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;
using AppointmentApi.Repository;
using AppointmentApi.Service;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AppointmentApi.Test;
public class PatientServiceTest
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(databaseName: "PatientTestDb")
            .Options;
        _context = new ClinicContext(options);
    }

    [Test]
    public async Task CreatePatient_Success()
    {
        // Arrange
        var mockPatientRepo = new Mock<IRepository<int, Patient>>();
        var mockUserRepo = new Mock<IRepository<string, User>>();
        var mockEncryptionService = new Mock<IEncryptionService>();
        var mockMapper = new Mock<IMapper>();
        var mockOtherFunc = new Mock<IOtherContextFunctionities>();

        var requestDto = new PatientAddRequestDTO
        {
            Password = "secret",
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com",
            Phone = "1234567890"
        };

        mockMapper.Setup(m => m.Map<PatientAddRequestDTO, User>(It.IsAny<PatientAddRequestDTO>()))
            .Returns(new User { Username = "john" });

        mockEncryptionService.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
            .ReturnsAsync(new EncryptModel
            {
                EncryptedData = new byte[] { 1, 2, 3 },
                HashKey = new byte[] { 4, 5, 6 }
            });

        mockUserRepo.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(new User { Username = "john" });

        mockPatientRepo.Setup(r => r.Add(It.IsAny<Patient>())).ReturnsAsync(new Patient
        {
            Id = 1,
            Name = "John Doe",
            Age = 30,
            Email = "john@example.com",
            Phone = "1234567890"
        });

        var service = new PatientService(
            mockPatientRepo.Object,
            mockUserRepo.Object,
            mockOtherFunc.Object,
            mockEncryptionService.Object,
            mockMapper.Object
        );

        // Act
        var result = await service.CreatePatient(requestDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo("John Doe"));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
