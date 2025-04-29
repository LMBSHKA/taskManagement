using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskService.Models;

namespace TaskService.Database
{
	public class JobAuditInterceptor : SaveChangesInterceptor
	{
		private readonly ILogger<JobAuditInterceptor> _logger;

		public JobAuditInterceptor(ILogger<JobAuditInterceptor> logger)
		{
			_logger = logger;
		}
		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
		{
			var context = eventData.Context;
			var historyEntries = new List<JobHistory>();

			foreach (var entry in context.ChangeTracker.Entries<Job>())
			{
				if (entry.State == EntityState.Added ||
				entry.State == EntityState.Modified ||
				entry.State == EntityState.Deleted)
				{
					var history = new JobHistory(
						entry.Entity.Id,
						entry.Entity.Name,
						entry.Entity.Status,
						entry.Entity.ExecutorName,
						entry.Entity.ExecutorSurname,
						entry.Entity.ExecutorId,
						entry.Entity.Description,
						entry.Entity.DeadLine!,
						entry.Entity.Priority,
						entry.Entity.IsDelete,
						entry.State.ToString(),
						DateTime.Now.ToString("dd.MM.yyyy")
						);

					historyEntries.Add(history);
				}
			}
			try
			{
				context.Set<JobHistory>().AddRange(historyEntries);

				return base.SavingChangesAsync(eventData, result, cancellationToken);
			}

			catch
			{
				_logger.LogCritical("Saving history failed");
				throw new Exception("saving history failed");
			}
		}
	}
}
