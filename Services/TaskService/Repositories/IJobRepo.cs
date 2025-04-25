using TaskService.DTOs;
using TaskService.Models;

namespace TaskService.Repositories
{
	public interface IJobRepo
	{
		Task<List<Job>> GetAllJobs(JobFilterDTO jobFilter, int pageNumber);
		Task<Job?> GetJobById(int id);
		Task<bool> CreateJob(Job job);
		Task<bool> UpdateJob(int id, UpdateJobDTO updateData);
		Task<bool> DeleteJob(int id);
		Task<bool> SetExecutor(int id, string executor);
	}
}
