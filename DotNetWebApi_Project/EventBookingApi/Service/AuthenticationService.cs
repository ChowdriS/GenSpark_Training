using System;
using EventBookingApi.Interface;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Service;
public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<string, User> _userRepository;

        public AuthenticationService(ITokenService tokenService,
                                    IEncryptionService encryptionService,
                                    IRepository<string, User> userRepository)
        {
            _tokenService = tokenService;
            _encryptionService = encryptionService;
            _userRepository = userRepository;
        }
        public async Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user)
        {
            try
            {
                var dbUser = await _userRepository.GetById(user.Email);
                if (dbUser == null || dbUser.IsDeleted)
                {
                    throw new Exception("No such user");
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, dbUser.PasswordHash);
                if(!isPasswordValid)
                    throw new Exception("Invalid password");
                var token = _tokenService.GenerateToken(dbUser);
                return new UserLoginResponseDTO
                {
                    Username = dbUser.Username??"",
                    Email = dbUser.Email??"",
                    Token = token,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
