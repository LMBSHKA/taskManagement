using TaskService.Models;

namespace TaskService.DTOs
{
	public class JobFilterDTO
	{
		public string? Name { get; set; } = String.Empty;
		public Status Status { get; set; } = Status.Null;
		public string? Executor { get; set; } = String.Empty;
		public string? Description { get; set; } = String.Empty;
		public DateTime DeadLine { get; set; } = DateTime.MinValue;
		public Priority Priority { get; set; } = Priority.Null;
	}
}
