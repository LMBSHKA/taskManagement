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
		public IActionResult GetAllJobs()
		{
			var jobs = _jobRepo.GetAllJobs().Result;
			if (jobs == null || jobs.Count == 0)
				return NotFound("Jobs not found");

			return Ok(jobs);
		}

		[HttpGet("tasks/{id}")]
		public IActionResult GetJobById(int id)
		{
			var job = _jobRepo.GetJobById(id).Result;
			if (job == null)
				return NotFound("job does not exist");

			return Ok(new { job.Name, job.Description, job.Status, job.DeadLine, job.Executor, job.Priority });
		}

		[HttpPost("tasks")]
		public IActionResult CreateJob([FromBody] Job newJob)
		{
			if (newJob == null)
				return BadRequest("Job data is invalid");
			
			if (_jobRepo.CreateJob(newJob).Result)
				return Ok("Job created");

			return BadRequest("Job does not created");
		}

		[HttpPut("tasks/{id}")]
		public IActionResult UpdateJob(int id, [FromBody] UpdateJobDTO updateData)
		{
			if (updateData == null)
				return BadRequest("Update data invalid");

			if (_jobRepo.UpdateJob(id, updateData).Result)
				return Ok();

			return BadRequest("Update failed");
		}

		[HttpDelete("tasks/{id}")]
		public IActionResult DeleteJob(int id)
		{
			if (_jobRepo.DeleteJob(id).Result)
				return Ok();

			return BadRequest("deletion failed");
		}

		[HttpPut("tasks/{id}/assign")]
		public IActionResult SetExecutor(int id, [FromBody] string executor)
		{
			if (String.IsNullOrEmpty(executor))
				return BadRequest("Executor's data invalid");

			if (_jobRepo.SetExecutor(id, executor).Result)
				return Ok();

			return BadRequest("The set is failed");
		}
	}
}
