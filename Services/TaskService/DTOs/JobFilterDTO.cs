using TaskService.Models;

namespace TaskService.DTOs
{
	public class JobFilterDTO
	{
		public string? Name { get; set; } = String.Empty;
		public string? ExecutorName { get; set; } = string.Empty;
		public string? ExecutorSurname { get; set; } = string.Empty;
		public string? Description { get; set; } = String.Empty;
	}
}
