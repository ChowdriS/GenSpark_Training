using System;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;

public class UserService : IUserService
{
    private readonly IRepository<Guid, User> _userRepository;
    private readonly IEncryptionService _encryptionService;

    public UserService(IRepository<Guid, User> userRepository,
                        IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    private async Task<User> Add(UserAddRequestDTO dto, UserRole role)
    {
        if (dto == null) throw new Exception("All fields are Required!");
        
        var passwordhash = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = dto.Password
        });

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordhash.EncryptedData,
            Role = role
        };

        user = await _userRepository.Add(user);
        return user;
    }

    public async Task<User> AddUser(UserAddRequestDTO dto)
    {
        return await Add(dto, UserRole.User);
    }

    public async Task<User> AddManager(UserAddRequestDTO dto)
    {
        return await Add(dto, UserRole.Manager);
    }

    public async Task<User> AddAdmin(UserAddRequestDTO dto)
    {
        return await Add(dto, UserRole.Admin);
    }
    public async Task<User> updateUser(Guid Id, UserUpdateRequestDTO dto)
    {
        var user = await _userRepository.GetById(Id);
        if (dto.Username != null)
        {
            user.Username = dto.Username;
            user.UpdatedAt = DateTime.UtcNow;
            user = await _userRepository.Update(Id, user);
        }
        else
        {
            throw new Exception("Nothing to update!");
        }
        return user;
    }

    public async Task<User> changePasssword(Guid Id, ChangePasswordDTO dto)
    {
        var user = await _userRepository.GetById(Id);
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.oldPassword,user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception("Invalid old password");
        var passwordhash = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = dto.newPassword
        });
        user.PasswordHash = passwordhash.EncryptedData;
        user.UpdatedAt = DateTime.UtcNow;
        user = await _userRepository.Update(Id, user);
        return user;
    }

    public async Task<User> deleteUser(Guid Id)
    {
        var user = await _userRepository.GetById(Id);
        if(user.IsDeleted)  throw new Exception("User is already deleted!");
        user.IsDeleted = true;
        user = await _userRepository.Update(Id, user);
        return user;
    }
}
