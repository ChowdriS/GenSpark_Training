using System;
using System.Security.Claims;
using EventBookingApi.Interface;

namespace EventBookingApi.Misc;

public class OtherFunctionalities : IOtherFunctionalities
{
    public Guid GetLoggedInUserId(ClaimsPrincipal User)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            // System.Console.WriteLine(userId);
            return userId;
        }
        else
        {
            throw new Exception("Invalid or missing user ID in token.");
        }
    }
}
