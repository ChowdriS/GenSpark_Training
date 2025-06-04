using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using AppointmentApi.Context;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using AppointmentApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApi.Tests
{
    public class DoctorRepositoryTests
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
        public async Task AddDoctorTest()
        {
            // Arrange
            var email = "test@gmail.com";
            var password = System.Text.Encoding.UTF8.GetBytes("test123");
            var key = Guid.NewGuid().ToByteArray();
            var user = new User
            {
                Username = email,
                Password = password,
                HashKey = key,
                Role = "Doctor"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var doctor = new Doctor
            {
                Name = "Dr. Test",
                YearsOfExperience = 3,
                Email = email
            };

            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act
            var result = await repo.Add(doctor);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetDoctorByIdTest()
        {
            // Arrange
            var doctor = new Doctor { Name = "Dr. Get", YearsOfExperience = 4, Email = "get@gmail.com" };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act
            var result = await repo.GetById(doctor.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(doctor.Id));
        }

        [Test]
        public async Task GetAllDoctorsTest()
        {
            // Arrange
            _context.Doctors.AddRange(
                new Doctor { Name = "Dr. A", YearsOfExperience = 2, Email = "a@gmail.com" },
                new Doctor { Name = "Dr. B", YearsOfExperience = 5, Email = "b@gmail.com" }
            );
            await _context.SaveChangesAsync();

            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act
            var result = await repo.GetAll();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task UpdateDoctorTest()
        {
            // Arrange
            var doctor = new Doctor { Name = "Dr. Update", YearsOfExperience = 1, Email = "update@gmail.com" };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            var updatedDoctor = new Doctor { Id = doctor.Id, Name = "Dr. Updated", YearsOfExperience = 10, Email = "updated@gmail.com" };

            // Act
            var result = await repo.Update(doctor.Id, updatedDoctor);

            // Assert
            Assert.That(result.Name, Is.EqualTo("Dr. Updated"));
            Assert.That(result.YearsOfExperience, Is.EqualTo(10));
        }

        [Test]
        public async Task DeleteDoctorTest()
        {
            // Arrange
            var doctor = new Doctor { Name = "Dr. Delete", YearsOfExperience = 7, Email = "delete@gmail.com" };
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act
            var result = await repo.Delete(doctor.Id);

            // Assert
            Assert.That(result.Id, Is.EqualTo(doctor.Id));
            Assert.That(await _context.Doctors.FindAsync(doctor.Id), Is.Null);
        }

        [Test]
        public void GetById_NotFound_ThrowsException()
        {
            // Arrange
            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetById(999));
            Assert.That(ex.Message, Is.EqualTo("No doctor with the given ID"));
        }

        [Test]
        public void GetAll_NoDoctors_ThrowsException()
        {
            // Arrange
            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await repo.GetAll());
            Assert.That(ex.Message, Is.EqualTo("No doctors in the database"));
        }

        [Test]
        public void Delete_NotFound_ThrowsException()
        {
            // Arrange
            IRepository<int, Doctor> repo = new DoctorRepository(_context);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await repo.Delete(999));
            Assert.That(ex.Message, Is.EqualTo("No doctor with the given ID"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
