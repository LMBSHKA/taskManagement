using System.ComponentModel.DataAnnotations;
using TaskService.DTOs;

namespace TaskService.Models
{
	public class Job
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string? Name { get; set; }
		public Status Status { get; set; } = Status.Created;
		public string? ExecutorName { get; set; } = "Not set";
		public string? ExecutorSurname { get; set; } = "Not set";
		public int ExecutorId { get; set; }
		public string? Description { get; set; } = string.Empty;
		[Required]
		public string? DeadLine { get; set; }
		[Required]
		public Priority Priority { get; set; }
		public bool IsDelete { get; set; }

		public Job() { }

		public Job (string name, Status status, string description,
			string deadLine, Priority priority, bool isDelete)
		{
			Name = name;
			Status = status;
			Description = description;
			DeadLine = deadLine;
			Priority = priority;
			IsDelete = isDelete;
		}
	}

	public enum Priority
	{
		Null = 0,
		Low = 1,
		Normal = 2,
		Major = 3,
		Critical = 4
	}

	public enum Status
	{
		Null = 0,
		Created = 1,
		InProgress = 2,
		Review = 3,
		Completed = 4
	}
}
