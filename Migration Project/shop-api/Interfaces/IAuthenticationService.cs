using System;
using shop_api.Models.DTO;

namespace shop_api.Interfaces;

public interface IAuthenticationService
{
    public Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user);
}