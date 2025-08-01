using System;
using shop_api.Models;

namespace shop_api.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
    public string GenerateRefreshToken();
}