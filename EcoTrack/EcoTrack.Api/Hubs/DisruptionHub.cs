using Microsoft.AspNetCore.SignalR;

namespace EcoTrack.Api.Hubs;

public class DisruptionHub : Hub
{
    // Clients will connect to this hub. 
    // We can add methods here later if the Angular client needs to push data back to the server.
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected to Disruption Hub: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }
}