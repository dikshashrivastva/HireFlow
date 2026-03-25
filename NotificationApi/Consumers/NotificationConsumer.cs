using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationApi.Hubs;
using NotificationApi.Models;

namespace NotificationApi.Consumers;

public class NotificationConsumer : IConsumer<NotificationMessage>
{
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly ILogger<NotificationConsumer> _logger;

	public NotificationConsumer(
		IHubContext<NotificationHub> hubContext,
		ILogger<NotificationConsumer> logger)
	{
		_hubContext = hubContext;
		_logger = logger;
	}

	public async Task Consume(ConsumeContext<NotificationMessage> context)
	{
		var message = context.Message;

		_logger.LogInformation(
			"Received notification for user {UserId}: {Message}",
			message.UserId,
			message.Message);

		// Push to the specific user's group via SignalR
		await _hubContext.Clients
			.Group($"user-{message.UserId}")
			.SendAsync("ReceiveNotification", message);
	}
}