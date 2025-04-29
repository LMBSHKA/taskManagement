using System.ComponentModel.DataAnnotations;

namespace TaskService.DTOs
{
	public class AsignExecutorDTO
	{
		[Required]
		public string? ExecutorName { get; set; }
		[Required]
		public string? ExecutorSurname { get; set; }
		[Required]
		public int ExecutorId { get; set; }
	}
}
