using System;
using shop_api.Interfaces;
using shop_api.Models;
using shop_api.Models.DTO;

namespace shop_api.Services;


public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IRepository<int, User> _userRepository;

    public AuthenticationService(ITokenService tokenService,
                                IRepository<int, User> userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }
    public async Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user)
    {
        try
        {
            var allUsers = await _userRepository.GetAll();

            var existingUser = allUsers.FirstOrDefault(u =>
                string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase));

            if (existingUser == null)
                throw new Exception("No such user");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password ?? "");
            if (!isPasswordValid)
                throw new Exception("Invalid password");

            var token = _tokenService.GenerateToken(existingUser);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.Update(existingUser.UserId, existingUser);

            return new UserLoginResponseDTO
            {
                Username = existingUser.Username ?? "",
                Token = token,
                RefreshToken = refreshToken
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
