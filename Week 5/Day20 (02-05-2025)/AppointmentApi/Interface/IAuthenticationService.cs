using System;
using AppointmentApi.Models.DTO;

namespace AppointmentApi.Interface;

public interface IAuthenticationService
{
    public Task<UserLoginResponse> Login(UserLoginRequest user);
}