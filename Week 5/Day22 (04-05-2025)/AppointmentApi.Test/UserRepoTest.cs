using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApi.Context;
using AppointmentApi.Models;
using AppointmentApi.Repository;
using AppointmentApi.Interface;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppointmentApi.Tests;
public class UserRepoTest
{
    private ClinicContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        _context = new ClinicContext(options);
    }

    [Test]
    public async Task AddUserTest()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser@gmail.com",
            Role = "Admin",
            Password = System.Text.Encoding.UTF8.GetBytes("password"),
            HashKey = Guid.NewGuid().ToByteArray()
        };

        IRepository<string, User> repo = new UserRepository(_context);

        // Act
        var result = await repo.Add(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Username, Is.EqualTo("testuser@gmail.com"));
    }

    [Test]
    public async Task GetUserById_ValidTest()
    {
        // Arrange
        var user = new User
        {
            Username = "user1@gmail.com",
            Role = "Doctor",
            Password = new byte[] { 1, 2, 3 },
            HashKey = new byte[] { 4, 5, 6 }
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        IRepository<string, User> repo = new UserRepository(_context);

        // Act
        var result = await repo.GetById("user1@gmail.com");

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Role, Is.EqualTo("Doctor"));
    }

    [Test]
    public async Task GetUserById_InvalidTest_ReturnsNull()
    {
        // Arrange
        IRepository<string, User> repo = new UserRepository(_context);

        // Act
        var result = await repo.GetById("nonexistent@gmail.com");

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetAllUsersTest()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Username = "a@gmail.com", Role = "Admin" },
            new User { Username = "b@gmail.com", Role = "Patient" }
        );
        await _context.SaveChangesAsync();

        IRepository<string, User> repo = new UserRepository(_context);

        // Act
        var result = await repo.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAll_NoUsers_ReturnsEmptyList()
    {
        // Arrange
        IRepository<string, User> repo = new UserRepository(_context);

        // Act
        var result = await repo.GetAll();

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Count(), Is.EqualTo(0));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}
