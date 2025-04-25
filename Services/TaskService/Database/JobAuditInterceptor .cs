using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskService.Models;

namespace TaskService.Database
{
	public class JobAuditInterceptor : SaveChangesInterceptor
	{
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
						entry.Entity.Executor,
						entry.Entity.Description,
						entry.Entity.DeadLine,
						entry.Entity.Priority,
						entry.Entity.IsDelete,
						entry.State.ToString(),
						DateTime.Now
						);

					historyEntries.Add(history);
				}
			}

			context.Set<JobHistory>().AddRange(historyEntries);

			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}
	}
}
