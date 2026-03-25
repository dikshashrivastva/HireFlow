using Microsoft.AspNetCore.SignalR;

namespace NotificationApi.Hubs;

public class NotificationHub : Hub
{
	// Called automatically when a browser/client connects
	public override async Task OnConnectedAsync()
	{
		var connectionId = Context.ConnectionId;
		Console.WriteLine($"Client connected: {connectionId}");
		await base.OnConnectedAsync();
	}

	// Called automatically when a browser/client disconnects
	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
		await base.OnDisconnectedAsync(exception);
	}

	// Client can call this method to join a specific group
	// For example: join group "user-123" to receive notifications for that user
	public async Task JoinUserGroup(string userId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
		Console.WriteLine($"Client {Context.ConnectionId} joined group user-{userId}");
	}
}