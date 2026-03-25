namespace NotificationApi.Models;

public class NotificationMessage
{
	public string UserId { get; set; } = string.Empty;
	public string Type { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public string CompanyName { get; set; } = string.Empty;
	public string JobTitle { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}