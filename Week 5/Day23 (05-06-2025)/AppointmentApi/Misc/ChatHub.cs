using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AppointmentApi.Misc;

public class ChatHub : Hub
{
    public async Task sendMessage(string username, string message)
    {
        await Clients.All.SendAsync("chatapp", username, message);
    } 
}
