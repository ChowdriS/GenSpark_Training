using System;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;
public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepository<Guid, User> _userRepository;

    public AuthenticationService(ITokenService tokenService,
                                IEncryptionService encryptionService,
                                IRepository<Guid, User> userRepository)
    {
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _userRepository = userRepository;
    }
    public async Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user)
    {
        try
        {
            var allUsers = await _userRepository.GetAll();

            var existingUser = allUsers.FirstOrDefault(u => 
                string.Equals(u.Email, user.Email, StringComparison.OrdinalIgnoreCase) && !u.IsDeleted);

            if (existingUser == null)
                throw new Exception("No such user");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash??"");
            if (!isPasswordValid)
                throw new Exception("Invalid password");

            var token = _tokenService.GenerateToken(existingUser);

            return new UserLoginResponseDTO
            {
                Username = existingUser.Username??"",
                Email = existingUser.Email??"",
                Token = token,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
