using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Database;
using NotificationService.DTO;
using NotificationService.Models;
using NotificationService.SignalR;
using System.Security.Claims;

namespace NotificationService.Repositories
{
	public class NotificationRepo : INotificationRepo
	{
		private readonly AppDbContext _context;

		public NotificationRepo(AppDbContext context)
		{ 
			_context = context;
		}

		public async Task<bool> CreateNotification(Notification notification)
		{
			
			if (notification == null)
				return false;

			try
			{
				notification.CreatedAt = DateTime.Now.ToString("dd.MM.yyyy");
				notification.IsRead = false;
				await _context.Notifications.AddAsync(notification);
				await _context.SaveChangesAsync();

				return true;
			}

			catch
			{
				return false;
			}
		}

		public async Task<List<GetNotificationDTO>> GetNotificationListByUserId(int userId)
		{
			var listNotification = await _context.Notifications
				.Where(x => x.UserId == userId)
				.Select(x => new GetNotificationDTO(x.JobId, x.UserId, x.Type, x.IsRead))
				.ToListAsync();

			return listNotification;
		}

		public async Task<bool> MarkAsRead(int id)
		{
			var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
			if (notification == null)
				return false;

			notification.IsRead = true;
			try
			{
				_context.Notifications.Update(notification);
				await _context.SaveChangesAsync();

				return true;
			}

			catch
			{
				return false;
			}
		}
	}
}
