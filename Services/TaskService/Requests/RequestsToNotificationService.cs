using System.Text.Json;
using System.Text;
using TaskService.DTOs;
using TaskService.Models;

namespace TaskService.Requests
{
	public class RequestsToNotificationService : IRequestsToNotificationService
	{
		private readonly ILogger _logger;

		public RequestsToNotificationService() { }

		public RequestsToNotificationService(ILogger<RequestsToNotificationService> logger)
		{
			_logger = logger;
		}

		public async Task RequestToCreateNotification(Job job, TypeNotification type)
		{
			if (job.ExecutorId > 0)
			{
				using (var httpClient = new HttpClient())
				{
					var notificationData = new RequestToNotificationDTO(job.Id, job.ExecutorId, type);
					var json = JsonSerializer.Serialize(notificationData);
					var content = new StringContent(json, Encoding.UTF8, "application/json");
					var url = "http://localhost:5165/api/notifications/";
					var response = await httpClient.PostAsync(url, content);

					if (response.IsSuccessStatusCode)
						_logger.LogInformation($"sending notification success, job id: {notificationData.JobId}, " +
							$"user id: {notificationData.UserId}, type: {notificationData.Type} " +
							$"response {response.Content}");

					else
						_logger.LogError($"sending notification failed, job id: {notificationData.JobId}, " +
							$"user id: {notificationData.UserId}, type: {notificationData.Type}, " +
							$"response content {response.Content}");
				}
			}

			else
				_logger.LogError($"sending notification failed, job id: {job.Id}, " +
					$"user id: {job.ExecutorId}, type: {type}");
		}
	}
}
