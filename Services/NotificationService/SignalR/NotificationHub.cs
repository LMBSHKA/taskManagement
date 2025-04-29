using Microsoft.AspNetCore.SignalR;
using NotificationService.DTO;

namespace NotificationService.SignalR
{
	public class NotificationHub : Hub
	{
		private readonly ILogger<NotificationHub> _logger;
		public NotificationHub() { }

		public NotificationHub(ILogger<NotificationHub> logger)
		{
			_logger = logger;
		}
		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var userId = httpContext!.Request.Query["userId"].ToString();
			if (userId != null)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, userId);
			}
			await base.OnConnectedAsync();
		}

		public async Task SendNotification(SendNotificationDTO notificationData)
		{
			var userId = notificationData.UserId;
			if (userId <= 0)
			{
				_logger.LogError($"Notification not send, user id: {notificationData.UserId}, " +
					$"message: {notificationData.Message}");

				throw new HubException("Notification not send: invalid user id");
			}

			_logger.LogInformation($"Notification send, user id: {notificationData.UserId}" +
				$"message: {notificationData.Message}");

			await Clients.Group(userId.ToString()).SendAsync("Notification", $"{notificationData.Message}");
		}
	}
}
