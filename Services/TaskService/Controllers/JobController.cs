using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TaskService.Database;
using TaskService.DTOs;
using TaskService.Models;
using TaskService.Repositories;

namespace TaskService.Controllers
{
	[Route("api/")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private readonly IJobRepo _jobRepo;

		public JobController(IJobRepo jobRepo)
		{
			_jobRepo = jobRepo;
		}

		[HttpGet("tasks")]
		public async Task<IActionResult> GetAllJobs([FromQuery] JobFilterDTO jobFilter, [FromQuery] int pageNumber = 1)
		{
			if (pageNumber <= 0)
				return BadRequest("Invalid page number");

			var jobs = await _jobRepo.GetAllJobs(jobFilter, pageNumber);
			if (jobs == null || jobs.Count == 0)
				return NotFound("Jobs not found");

			return Ok(jobs);
		}

		[HttpGet("tasks/{id}")]
		public async Task<IActionResult> GetJobById(int id)
		{
			var job = await _jobRepo.GetJobById(id);
			if (job == null)
				return NotFound("job does not exist");

			return Ok(new { job.Name, job.Description, job.Status, job.DeadLine, job.ExecutorName, job.ExecutorSurname, job.Priority });
		}

		[HttpPost("tasks")]
		public async Task<IActionResult> CreateJob([FromBody] Job newJob)
		{
			if (newJob == null)
				return BadRequest("Job data is invalid");
			
			if (await _jobRepo.CreateJob(newJob))
				return Ok("Job created");

			return BadRequest("Job does not created");
		}

		[HttpPut("tasks/{id}")]
		public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDTO updateData)
		{
			if (updateData == null)
				return BadRequest("Update data invalid");

			if (await _jobRepo.UpdateJob(id, updateData))
				return Ok();

			return BadRequest("Update failed");
		}

		[HttpDelete("tasks/{id}")]
		public async Task<IActionResult> DeleteJob(int id)
		{
			if (await _jobRepo.DeleteJob(id))
				return Ok();

			return BadRequest("deletion failed");
		}

		[HttpPut("tasks/{id}/assign")]
		public async Task<IActionResult> SetExecutor(int id, [FromBody] AsignExecutorDTO executor)
		{
			if (executor == null)
				return BadRequest("Executor's data invalid");

			if (executor.ExecutorId <= 0)
				return BadRequest("Executor id invalid");

			if (await _jobRepo.AsignExecutor(id, executor))
				return Ok();

			return BadRequest("The set is failed");
		}
	}
}
