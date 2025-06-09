using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Security.Claims;
namespace EventBookingApi.Misc;

public class NotificationHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (groupName == $"user_{userId}" ||
            groupName == "admins" && Context.User.IsInRole("Admin") ||
            groupName.StartsWith("manager_") && Context.User.IsInRole("Manager"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}