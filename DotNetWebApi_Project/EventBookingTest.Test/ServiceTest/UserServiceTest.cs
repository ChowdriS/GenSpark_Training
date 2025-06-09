using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventBookingApi.Test
{
    public class UserServiceTest
    {
        private Mock<IRepository<Guid, User>> _userRepo;
        private Mock<IEncryptionService> _encryptionService;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepo = new Mock<IRepository<Guid, User>>();
            _encryptionService = new Mock<IEncryptionService>();

            _userService = new UserService(_userRepo.Object, _encryptionService.Object);
        }

        [Test]
        public void AddUser_NullDto_ThrowsException()
        {
            var ex = Assert.ThrowsAsync<Exception>(() => _userService.AddUser(null));
            Assert.That(ex.Message, Is.EqualTo("All fields are Required!"));
        }

        [Test]
        public async Task AddUser_ValidDto_ReturnsUser()
        {
            var dto = new UserAddRequestDTO
            {
                Username = "chowdri",
                Email = "chowdri@example.com",
                Password = "secure"
            };

            _encryptionService.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                .ReturnsAsync(new EncryptModel { EncryptedData = "hashed" });

            _userRepo.Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            var result = await _userService.AddUser(dto);

            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(dto.Email));
            Assert.That(result.Role, Is.EqualTo(UserRole.User));
        }

        [Test]
        public async Task AddManager_ValidDto_AssignsManagerRole()
        {
            var dto = new UserAddRequestDTO
            {
                Username = "mgr",
                Email = "mgr@example.com",
                Password = "pwd"
            };

            _encryptionService.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                .ReturnsAsync(new EncryptModel { EncryptedData = "hashed" });

            _userRepo.Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            var result = await _userService.AddManager(dto);

            Assert.That(result.Role, Is.EqualTo(UserRole.Manager));
        }

        [Test]
        public async Task UpdateUser_ValidUsername_UpdatesSuccessfully()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id, Username = "old" };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);
            _userRepo.Setup(r => r.Update(id, It.IsAny<User>()))
                     .ReturnsAsync((Guid _, User u) => u);

            var dto = new UserUpdateRequestDTO { Username = "new" };

            var updatedUser = await _userService.updateUser(id, dto);

            Assert.That(updatedUser.Username, Is.EqualTo("new"));
        }

        [Test]
        public void UpdateUser_EmptyDto_ThrowsException()
        {
            var id = Guid.NewGuid();
            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(new User());

            var dto = new UserUpdateRequestDTO { Username = null };

            var ex = Assert.ThrowsAsync<Exception>(() => _userService.updateUser(id, dto));
            Assert.That(ex.Message, Is.EqualTo("Nothing to update!"));
        }

        [Test]
        public async Task ChangePassword_ValidOldPassword_UpdatesPassword()
        {
            var id = Guid.NewGuid();
            var oldHashed = BCrypt.Net.BCrypt.HashPassword("oldpass");

            var user = new User { PasswordHash = oldHashed };
            var dto = new ChangePasswordDTO { oldPassword = "oldpass", newPassword = "newpass" };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);
            _encryptionService.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                .ReturnsAsync(new EncryptModel { EncryptedData = "newHashed" });
            _userRepo.Setup(r => r.Update(id, It.IsAny<User>())).ReturnsAsync((Guid _, User u) => u);

            var updated = await _userService.changePasssword(id, dto);

            Assert.That(updated.PasswordHash, Is.EqualTo("newHashed"));
        }

        [Test]
        public void ChangePassword_WrongOldPassword_ThrowsException()
        {
            var id = Guid.NewGuid();
            var user = new User { PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct") };
            var dto = new ChangePasswordDTO { oldPassword = "wrong", newPassword = "new" };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);

            var ex = Assert.ThrowsAsync<Exception>(() => _userService.changePasssword(id, dto));
            Assert.That(ex.Message, Is.EqualTo("Invalid old password"));
        }

        [Test]
        public async Task DeleteUser_NotYetDeleted_SoftDeletes()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id, IsDeleted = false };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);
            _userRepo.Setup(r => r.Update(id, It.IsAny<User>())).ReturnsAsync((Guid _, User u) => u);

            var result = await _userService.deleteUser(id);

            Assert.IsTrue(result.IsDeleted);
        }

        [Test]
        public void DeleteUser_AlreadyDeleted_ThrowsException()
        {
            var id = Guid.NewGuid();
            var user = new User { Id = id, IsDeleted = true };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);

            var ex = Assert.ThrowsAsync<Exception>(() => _userService.deleteUser(id));
            Assert.That(ex.Message, Is.EqualTo("User is already deleted!"));
        }

        [Test]
        public async Task GetMe_ValidUserId_ReturnsUserDetails()
        {
            var id = Guid.NewGuid();
            var user = new User { Username = "chowdri", Email = "chowdri@example.com", Role = UserRole.Admin };

            _userRepo.Setup(r => r.GetById(id)).ReturnsAsync(user);

            var result = await _userService.GetMe(id);

            Assert.That(result.Username, Is.EqualTo("chowdri"));
            Assert.That(result.Role, Is.EqualTo("Admin"));
        }
    }
}
