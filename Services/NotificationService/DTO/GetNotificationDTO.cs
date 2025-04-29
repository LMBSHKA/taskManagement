using NotificationService.Models;

namespace NotificationService.DTO
{
	public class GetNotificationDTO
	{
		public int JobId { get; set; }
		public int UserId { get; set; }
		public TypeNotification Type { get; set; }
		public bool IsRead { get; set; }

		public GetNotificationDTO() { }

		public GetNotificationDTO(int jobId, int userId, TypeNotification type, bool isRead)
		{
			JobId = jobId;
			UserId = userId;
			Type = type;
			IsRead = isRead;
		}
	}
}
