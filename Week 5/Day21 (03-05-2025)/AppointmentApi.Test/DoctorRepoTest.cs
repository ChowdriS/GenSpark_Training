using AppointmentApi.Context;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApi.Test;

public class Tests
{
    private ClinicContext _context;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
                            .UseInMemoryDatabase("TestDb")
                            .Options;
        _context = new ClinicContext(options);
    }

    [Test]
    public async Task AddDoctorTest()
    {
        //arrange
        var email = " test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();
        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        var result = await _doctorRepository.Add(doctor);
        //assert
        Assert.That(result, Is.Not.Null, "Doctor IS not addeed");
        Assert.That(result.Id, Is.EqualTo(1));
    }

    [TestCase(999)]
    [TestCase(1)]
    public async Task GetDoctorPassTest(int id)
    {
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        var result = await _doctorRepository.GetById(id);
        //assert
        Assert.That(result.Id, Is.EqualTo(id));

    }

    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
    }
}