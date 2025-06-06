using System;
using EventBookingApi.Model;
using EventBookingApi.Model.DTO;

namespace EventBookingApi.Interface;

public interface IAuthenticationService
{
    public Task<UserLoginResponseDTO> Login(UserLoginRequestDTO user);
}