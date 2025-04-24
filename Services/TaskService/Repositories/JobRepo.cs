using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using TaskService.Database;
using TaskService.DTOs;
using TaskService.Models;

namespace TaskService.Repositories
{
	public class JobRepo : IJobRepo
	{
		private readonly AppDbContext _context;

		public JobRepo (AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Job>> GetAllJobs()
		{
			return await _context.Jobs.Where(x => x.IsDelete == false).ToListAsync();
		}

		public async Task<Job?> GetJobById(int id)
		{
			return await _context.Jobs.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);
		}

		public async Task<bool> CreateJob(Job job)
		{
			try
			{
				job.IsDelete = false;
				await _context.Jobs.AddAsync(job);
				await _context.SaveChangesAsync();

				return true;
			}

			catch
			{
				return false;
			}
		}

		public async Task<bool> UpdateJob(int id, UpdateJobDTO updateData)
		{
			var job = await _context.Jobs.FindAsync(id);

			if (job == null || job.IsDelete == true)
				return false;

			Update(job, updateData);
			try
			{
				_context.Jobs.Update(job);
				await _context.SaveChangesAsync();

				return true;
			}

			catch
			{
				return false;
			}
		}

		private void Update(Job job, UpdateJobDTO updateData)
		{
			job.Name = String.IsNullOrEmpty(updateData.Name) ? job.Name : updateData.Name;
			job.Status = updateData.Status == Status.Null ? job.Status : updateData.Status;
			job.DeadLine = updateData.DeadLine == DateTime.MinValue ? job.DeadLine : updateData.DeadLine;
			job.Description = String.IsNullOrEmpty(updateData.Description) ? job.Description : updateData.Description;
			job.Priority = updateData.Priority == Priority.Null ? job.Priority : updateData.Priority;
		}

		public async Task<bool> DeleteJob(int id)
		{
			var job = await _context.Jobs.FindAsync(id);

			if (job == null)
				return false;

			job.IsDelete = true;
			_context.Jobs.Update(job);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> SetExecutor(int id, string executor)
		{
			var job = await _context.Jobs.FindAsync(id);

			if (job == null || job.IsDelete == true)
				return false;

			job.Executor = executor;

			try
			{
				_context.Jobs.Update(job);
				await _context.SaveChangesAsync();

				return true;
			}

			catch
			{
				return false;
			}
		}
	}
}