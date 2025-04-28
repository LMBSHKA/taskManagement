using Microsoft.AspNetCore.Mvc.RazorPages;
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
		private readonly int _pageSize = 20;

		public JobRepo (AppDbContext context)
		{
			_context = context;
		}

		public async Task<List<Job>> GetAllJobs(JobFilterDTO jobFilter, int pageNumber)
		{
			var jobs = SetFilterAndPagination(pageNumber, jobFilter);

			return await jobs.OrderBy(job => job.Name).ToListAsync();
		}

		private IQueryable<Job> SetFilterAndPagination(int pageNumber, JobFilterDTO jobFilter)
		{
			var jobs = _context.Jobs.Where(job => job.IsDelete == false &&
			EF.Functions.Like(job.Name!, $"%{jobFilter.Name}%") &
			EF.Functions.Like(job.Status.ToString(), $"%{(jobFilter.Status == Status.Null ? "" : jobFilter.Status)}%") &
			EF.Functions.Like(job.ExecutorName!, $"%{jobFilter.ExecutorName}%") &
			EF.Functions.Like(job.ExecutorSurname!, $"%{jobFilter.ExecutorSurname}%") &
			EF.Functions.Like(job.Description!, $"%{jobFilter.Description}%") &
			EF.Functions.Like(job.DeadLine.ToString()!, $"%{(jobFilter.DeadLine == DateTime.MinValue ? "" : jobFilter.DeadLine)}%") &
			EF.Functions.Like(job.Priority.ToString()!, $"%{(jobFilter.Priority == Priority.Null ? "" : jobFilter.Priority)}%")
			);
			var startIndex = (pageNumber - 1) * _pageSize;
			var pagedItems = jobs.Skip(startIndex).Take(_pageSize);

			return pagedItems;
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

		//TODO Send notify
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

		//TODO Send notify
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

		//TODO Send notify
		public async Task<bool> AsignExecutor(int id, AsignExecutorDTO executor)
		{
			var job = await _context.Jobs.FindAsync(id);

			if (job == null || job.IsDelete == true)
				return false;

			job.ExecutorName = executor.ExecutorName;
			job.ExecutorSurname = executor.ExecutorSurname;
			job.ExecutorId = executor.ExecutorId;

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