using System;
using System.Security.Claims;

namespace shop_api.Interfaces;

public interface IOtherFunctionalities
{
    public int GetLoggedInUserId(ClaimsPrincipal User);
}