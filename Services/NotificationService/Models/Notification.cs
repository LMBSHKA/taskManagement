using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models
{
	public class Notification
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int JobId { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public TypeNotification Type { get; set; }
		[Required]
		public bool IsRead { get; set; } = false;
		public string? CreatedAt { get; set; }
	}

	public enum TypeNotification
	{
		Null = 0,
		SetExecutor,
		UpdateJob,
		DeleteJob
	}
}
