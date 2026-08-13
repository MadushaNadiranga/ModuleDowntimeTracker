using Microsoft.AspNetCore.SignalR;

namespace DowntimeTracker.Api.Hubs;

public class DowntimeHub : Hub
{
    // Clients call this after connecting to join updates for a specific module
    public async Task JoinModuleGroup(string moduleId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"module-{moduleId}");
    }

    public async Task LeaveModuleGroup(string moduleId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"module-{moduleId}");
    }
}