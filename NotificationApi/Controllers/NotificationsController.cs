using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Models;

namespace NotificationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
	private readonly IPublishEndpoint _publishEndpoint;

	public NotificationsController(IPublishEndpoint publishEndpoint)
	{
		_publishEndpoint = publishEndpoint;
	}

	[HttpGet("health")]
	public IActionResult Health()
	{
		return Ok("HireFlow API is running!");
	}

	// Simulates a recruiter updating an application status
	[HttpPost("update-status")]
	public async Task<IActionResult> UpdateApplicationStatus(
		[FromBody] NotificationMessage message)
	{
		message.Timestamp = DateTime.UtcNow;
		await _publishEndpoint.Publish<NotificationMessage>(message);
		return Ok(new
		{
			success = true,
			message = $"Status updated to '{message.Status}' for {message.UserId}"
		});
	}

	// Simulates multiple updates at once — like a recruiter bulk reviewing
	[HttpPost("bulk-update")]
	public async Task<IActionResult> BulkUpdate(
		[FromBody] List<NotificationMessage> messages)
	{
		foreach (var message in messages)
		{
			message.Timestamp = DateTime.UtcNow;
			await _publishEndpoint.Publish<NotificationMessage>(message);
		}
		return Ok(new
		{
			success = true,
			message = $"{messages.Count} notifications sent"
		});
	}
}