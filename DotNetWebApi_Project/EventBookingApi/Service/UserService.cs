using System;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;

public class UserService : IUserService
{
    private readonly IRepository<string, User> _userRepository;
    private readonly IEncryptionService _encryptionService;

    public UserService(IRepository<string, User> userRepository,
                        IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<User> Add(UserAddRequestDTO dto)
    {
        try
        {
            if (dto == null) throw new Exception("All fields are Required!");
            var passwordhash = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = dto.Password
            });
            User user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordhash.EncryptedData,
                Role = dto.Role
            };

            user = await _userRepository.Add(user);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<User> addUser(UserAddRequestDTO dto)
    {
        try
        {
            if (dto.Role != "user" && dto.Role != "manager") throw new Exception("The Role is not valid.");
            var user = await Add(dto);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<User> addAdmin(UserAddRequestDTO dto)
    {
        try
        {
            if (dto.Role != "admin") throw new Exception("The Role is not valid");
            var user = await Add(dto);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }

    public async Task<User> updateUser(string email, UserUpdateRequestDTO dto)
    {
        try
        {
            var user = await _userRepository.GetById(email);
            if (dto.Username != null)
            {
                user.Username = dto.Username;
                user = await _userRepository.Update(email, user);
            }
            else
            {
                throw new Exception("Nothing to update!");
            }
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> changePasssword(string email, ChangePasswordDTO dto)
    {
        try
        {
            var user = await _userRepository.GetById(email);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.oldPassword,user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Invalid old password");
            var passwordhash = await _encryptionService.EncryptData(new EncryptModel
            {
                Data = dto.newPassword
            });
            user.PasswordHash = passwordhash.EncryptedData;
            user = await _userRepository.Update(email, user);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> deleteUser(string email)
    {
        try
        {
            var user = await _userRepository.GetById(email);
            if(user.IsDeleted)  throw new Exception("User is already deleted!");
            user.IsDeleted = true;
            user = await _userRepository.Update(email, user);
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        
    }
}
