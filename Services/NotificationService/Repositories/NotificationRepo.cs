using Microsoft.EntityFrameworkCore;
using NotificationService.Database;
using NotificationService.DTO;
using NotificationService.Models;

namespace NotificationService.Repositories
{
	public class NotificationRepo : INotificationRepo
	{
		private readonly ILogger<NotificationRepo> _logger;
		private readonly AppDbContext _context;

		public NotificationRepo(AppDbContext context, ILogger<NotificationRepo> logger)
		{ 
			_logger = logger;
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
				_logger.LogWarning($"Notifiacation not created, user id: {notification.UserId}, " +
					$"job id: {notification.JobId}, type: {notification.Type}");
				return false;
			}
		}

		public async Task<List<GetNotificationDTO>> GetNotificationListByUserId(int userId)
		{
			try
			{
				var listNotification = await _context.Notifications
					.Where(x => x.UserId == userId)
					.Select(x => new GetNotificationDTO(x.JobId, x.UserId, x.Type, x.IsRead))
					.ToListAsync();

				return listNotification;
			}

			catch 
			{
				_logger.LogWarning($"Can't get notification, user id: {userId}");
				return null!;
			}
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
				_logger.LogWarning($"Notifcation {id}, not reading");
				return false;
			}
		}
	}
}
