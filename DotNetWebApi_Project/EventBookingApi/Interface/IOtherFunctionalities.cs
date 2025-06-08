using System;
using System.Security.Claims;

namespace EventBookingApi.Interface;

public interface IOtherFunctionalities
{
    public Guid GetLoggedInUserId(ClaimsPrincipal User);
}
