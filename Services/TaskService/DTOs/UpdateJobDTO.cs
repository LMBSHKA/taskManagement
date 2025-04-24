using System.ComponentModel.DataAnnotations;
using TaskService.Models;

namespace TaskService.DTOs
{
	public class UpdateJobDTO
	{
		public string Name { get; set; } = string.Empty;
		public Status Status { get; set; } = Status.Null;
		public string Description { get; set; } = String.Empty;
		public DateTime DeadLine { get; set; } = DateTime.MinValue;
		public Priority Priority { get; set; } = Priority.Null;
	}
}