using System;
using System.Security.Claims;
using shop_api.Interfaces;

namespace shop_api.misc;

public class OtherFunctionalities : IOtherFunctionalities
{
    private readonly ObjectMapper _mapper;

    public OtherFunctionalities( ObjectMapper mapper)
    {
        _mapper = mapper;
    }
    public int GetLoggedInUserId(ClaimsPrincipal User)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }
        else
        {
            throw new Exception("Invalid or missing user ID in token.");
        }
    }
}
