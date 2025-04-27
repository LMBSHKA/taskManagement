using NotificationService.DTO;
using NotificationService.Models;

namespace NotificationService.Repositories
{
	public interface INotificationRepo
	{
		Task<bool> CreateNotification(Notification notification);
		Task<List<GetNotificationDTO>> GetNotificationListByUserId(int userId);
		Task<bool> MarkAsRead(int id);
	}
}
