using System.ComponentModel.DataAnnotations;
using TaskService.DTOs;

namespace TaskService.Models
{
	public class Job
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public Status Status { get; set; }
		public string? Executor { get; set; }
		public string? Description { get; set; }
		public DateTime DeadLine { get; set; }
		public Priority Priority { get; set; }
		public bool IsDelete { get; set; }

		public Job() { }

		public Job (string name, Status status, string executor, string description,
			DateTime deadLine, Priority priority, bool isDelete)
		{
			Name = name;
			Status = status;
			Executor = executor;
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
