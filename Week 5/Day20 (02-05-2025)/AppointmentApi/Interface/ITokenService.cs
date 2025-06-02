using System;
using AppointmentApi.Models;

namespace AppointmentApi.Interface;

public interface ITokenService
{
    public Task<string> GenerateToken(User user);
}