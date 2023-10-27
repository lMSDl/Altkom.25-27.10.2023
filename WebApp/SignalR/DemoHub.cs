using Microsoft.AspNetCore.SignalR;

namespace WebApp.SignalR
{
    public class DemoHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();


            Console.WriteLine(Context.ConnectionId);
            await Clients.Caller.SendAsync("TextMessage", "Welcome in signalR!");
        }

        public Task SayHelloToOthers(string message)
        {
            return Clients.Others.SendAsync("TextMessage", message);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Groups(groupName).SendAsync("TextMessage", $"New member in group {groupName}: {Context.ConnectionId}");
        }
    }
}
