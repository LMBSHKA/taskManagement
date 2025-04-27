using Microsoft.AspNetCore.SignalR;
using NotificationService.DTO;
using NotificationService.Models;
using System.Security.Claims;

namespace NotificationService.SignalR
{
	public class NotificationHub : Hub
	{
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
				throw new HubException("Notification not send: invalid user id");

			await Clients.Group(userId.ToString()).SendAsync("Notification", $"{notificationData.Message}");
		}
	}
}
