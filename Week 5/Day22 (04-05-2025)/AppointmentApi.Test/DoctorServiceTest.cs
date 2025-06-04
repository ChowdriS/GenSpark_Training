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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Test
{
    public class DoctorServiceTest
    {
        private ClinicContext _context;
        private Mock<IRepository<int, Doctor>> _doctorRepo;
        private Mock<IRepository<int, Speciality>> _specialityRepo;
        private Mock<IRepository<int, DoctorSpeciality>> _doctorSpecialityRepo;
        private Mock<IRepository<string, User>> _userRepo;
        private Mock<IOtherContextFunctionities> _otherFunc;
        private Mock<IEncryptionService> _encryptService;
        private Mock<IMapper> _mapper;
        private IDoctorService _doctorService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ClinicContext(options);
            _doctorRepo = new Mock<IRepository<int, Doctor>>();
            _specialityRepo = new Mock<IRepository<int, Speciality>>();
            _doctorSpecialityRepo = new Mock<IRepository<int, DoctorSpeciality>>();
            _userRepo = new Mock<IRepository<string, User>>();
            _otherFunc = new Mock<IOtherContextFunctionities>();
            _encryptService = new Mock<IEncryptionService>();
            _mapper = new Mock<IMapper>();

            _doctorService = new DoctorService(
                _doctorRepo.Object,
                _specialityRepo.Object,
                _doctorSpecialityRepo.Object,
                _userRepo.Object,
                _otherFunc.Object,
                _encryptService.Object,
                _mapper.Object
            );
        }

        [Test]
        public async Task AddDoctor_ShouldAddDoctorAndUserWithSpecialities()
        {
            var doctorDto = new DoctorAddRequestDto
            {
                Name = "Dr. Strange",
                Email = "strange@ex.com",
                Password = "magic",
                Specialities = new List<SpecialityAddRequestDTO> { new() { Name = "Neuro" } },
                YearsOfExperience = 10
            };

            var encrypted = new EncryptModel
            {
                EncryptedData = new byte[] { 1, 2, 3 },
                HashKey = new byte[] { 4, 5, 6 }
            };

            _encryptService.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encrypted);
            _mapper.Setup(m => m.Map<DoctorAddRequestDto, User>(doctorDto)).Returns(new User { Username = doctorDto.Email });
            _userRepo.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(new User { Username = doctorDto.Email });
            _doctorRepo.Setup(r => r.Add(It.IsAny<Doctor>())).ReturnsAsync(new Doctor { Id = 1, Name = doctorDto.Name });
            _specialityRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<Speciality>());
            _specialityRepo.Setup(r => r.Add(It.IsAny<Speciality>())).ReturnsAsync(new Speciality { Id = 1, Name = "Neuro" });
            _doctorSpecialityRepo.Setup(r => r.Add(It.IsAny<DoctorSpeciality>())).ReturnsAsync(new DoctorSpeciality { DoctorId = 1, SpecialityId = 1 });

            var result = await _doctorService.AddDoctor(doctorDto);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Dr. Strange"));
        }

        [TestCase("Dr. D")]
        public async Task GetDoctorByName_ReturnsDoctor(string name)
        {
            var doctors = new List<Doctor> { new() { Name = name } };
            _doctorRepo.Setup(r => r.GetAll()).ReturnsAsync(doctors);

            var result = await _doctorService.GetDoctorByName(name);

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public void GetDoctorByName_NotFound_ThrowsException()
        {
            _doctorRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<Doctor>());

            var ex = Assert.ThrowsAsync<Exception>(() => _doctorService.GetDoctorByName("Invalid"));
            Assert.That(ex.Message, Is.EqualTo("No doctor found with the given name"));
        }

        [TestCase("Cardiology")]
        public async Task GetDoctorsBySpeciality_ReturnsMappedDtos(string speciality)
        {
            _otherFunc.Setup(f => f.GetDoctorsBySpeciality(speciality))
                      .ReturnsAsync(new List<DoctorsBySpecialityResponseDto>
                      {
                          new DoctorsBySpecialityResponseDto { Dname = "Dr. A", Id = 1, Yoe = 5 }
                      });

            var result = await _doctorService.GetDoctorsBySpeciality(speciality);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Dname, Is.EqualTo("Dr. A"));
        }

        [TearDown]
        public void TearDown() => _context.Dispose();
    }
}
