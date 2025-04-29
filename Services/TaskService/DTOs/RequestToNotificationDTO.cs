
namespace TaskService.DTOs
{
	public class RequestToNotificationDTO
	{
		public int JobId { get; set; }
		public int UserId { get; set; }
		public TypeNotification Type { get; set; }

		public RequestToNotificationDTO() { }

		public RequestToNotificationDTO(int jobId, int userId, TypeNotification type)
		{
			JobId = jobId;
			UserId = userId;
			Type = type;
		}
	}

	public enum TypeNotification
	{
		Null = 0,
		SetExecutor,
		UpdateJob,
		DeleteJob
	}
}
