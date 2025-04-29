using Microsoft.EntityFrameworkCore;
using TaskService.Database;
using TaskService.DTOs;
using TaskService.Models;
using TaskService.Requests;

namespace TaskService.Repositories
{
	public class JobRepo : IJobRepo
	{
		private readonly AppDbContext _context;
		private readonly IRequestsToNotificationService _requestsToNotificationService;
		private int _pageSize { get; } = 20;

		public JobRepo(AppDbContext context, IRequestsToNotificationService requestsToNotificationService)
		{
			_requestsToNotificationService = requestsToNotificationService;
			_context = context;
		}

		public async Task<List<Job>> GetAllJobs(JobFilterDTO jobFilter, int pageNumber)
		{
			var jobs = SetFilterAndPagination(pageNumber, jobFilter);

			return await jobs.OrderBy(job => job.Name).ToListAsync();
		}

		private IQueryable<Job> SetFilterAndPagination(int pageNumber, JobFilterDTO jobFilter)
		{
			var jobs = _context.Jobs
				.Where(job => 
				job.IsDelete == false && 
				EF.Functions.Like(job.Name!, $"%{jobFilter.Name}%") &
				EF.Functions.Like(job.ExecutorName!, $"%{jobFilter.ExecutorName}%") &
				EF.Functions.Like(job.ExecutorSurname!, $"%{jobFilter.ExecutorSurname}%") &
				EF.Functions.Like(job.Description!, $"%{jobFilter.Description}%")
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

				await _requestsToNotificationService.RequestToCreateNotification(job, TypeNotification.UpdateJob);

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
			job.DeadLine = updateData.DeadLine == DateTime.MinValue ? job.DeadLine : updateData.DeadLine.ToString("dd.MM.yyyy");
			job.Description = String.IsNullOrEmpty(updateData.Description) ? job.Description : updateData.Description;
			job.Priority = updateData.Priority == Priority.Null ? job.Priority : updateData.Priority;
		}

		public async Task<bool> DeleteJob(int id)
		{
			var job = await _context.Jobs.FindAsync(id);

			if (job == null)
				return false;

			job.IsDelete = true;
			try
			{
				_context.Jobs.Update(job);
				await _context.SaveChangesAsync();

				await _requestsToNotificationService.RequestToCreateNotification(job, TypeNotification.DeleteJob);

				return true;
			}

			catch
			{
				return false;
			}
		}

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

				await _requestsToNotificationService.RequestToCreateNotification(job, TypeNotification.SetExecutor);

				return true;
			}

			catch
			{
				return false;
			}
		}
	}
}