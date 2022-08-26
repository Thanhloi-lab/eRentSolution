using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Hubs
{
    public class OnlineCountHub : Hub
    {
        private static int Count = 0;
        public override Task OnConnectedAsync()
        {
            Count++;
            base.OnConnectedAsync();
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Count--;
            base.OnDisconnectedAsync(exception);
            Clients.All.SendAsync("updateCount", Count);
            return Task.CompletedTask;
        }
    }
}
