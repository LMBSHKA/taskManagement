using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Repositories;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
	[ApiController]
	[Route("api/notifications/")]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationRepo _notificationRepo;

		public NotificationController(INotificationRepo notificationRepo)
		{
			_notificationRepo = notificationRepo;
		}

		[HttpPost]
		public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
		{
			if (await _notificationRepo.CreateNotification(notification))
				return Ok("notification created");

			return BadRequest("notification doesn't created");
		}

		[HttpGet("{userId}")]
		public async Task<IActionResult> GetNotificationListByUserId(int userId)
		{
			var listNotification = await _notificationRepo.GetNotificationListByUserId(userId);
			if (listNotification == null)
				return NotFound("Notification not found");

			return Ok(listNotification);
		}

		[HttpPut("{id}/mark-as-read")]
		public async Task<IActionResult> MarksAsRead(int id)
		{
			if (id <= 0)
				return BadRequest("Invalid id");

			if (await _notificationRepo.MarkAsRead(id))
				return Ok();

			return BadRequest("Reading failed");
		}
	}
}
