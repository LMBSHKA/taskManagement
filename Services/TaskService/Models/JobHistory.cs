using System.ComponentModel.DataAnnotations;

namespace TaskService.Models
{
	public class JobHistory
	{
		[Key]
		public int Id { get; set; }
		public int JobId { get; set; }
		public string? Name { get; set; }
		public Status Status { get; set; }
		public string? ExecutorName { get; set; }
		public string? ExecutorSurname { get; set; }
		public int ExecutorId { get; set; }
		public string? Description { get; set; }
		public DateTime DeadLine { get; set; }
		public Priority Priority { get; set; }
		public bool IsDelete { get; set; }
		public string? OperationType { get; set; }
		public DateTime ChangedAt { get; set; }

		public JobHistory() { }

		public JobHistory(int jobId, 
			string? name, 
			Status status, 
			string? executorName, 
			string? executorSurname,
			int executorId,
			string? description, 
			DateTime deadLine, 
			Priority priority, 
			bool isDelete, 
			string? operationType, 
			DateTime changedAt)
		{
			JobId = jobId;
			Name = name;
			Status = status;
			ExecutorName = executorName;
			ExecutorSurname = executorSurname;
			ExecutorId = executorId;
			Description = description;
			DeadLine = deadLine;
			Priority = priority;
			IsDelete = isDelete;
			OperationType = operationType;
			ChangedAt = changedAt;
		}
	}
}
