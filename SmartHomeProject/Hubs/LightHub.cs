using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeProject.Hubs
{
    public class LightHub: Hub
    {
        public async Task SendMessage(int result, string command)
        {
            await Clients.All.SendAsync("ReceiveMessage", result, command);
        }
    }
}
