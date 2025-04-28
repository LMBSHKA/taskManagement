using System.Text.Json;
using System.Text;
using TaskService.DTOs;
using TaskService.Models;

namespace TaskService.Requests
{
	public class RequestsToNotificationService : IRequestsToNotificationService
	{
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
						Console.WriteLine("Notification created");

					else
						Console.WriteLine("Notification creation failed");
				}
			}

			else
				Console.WriteLine("Notification creation failed");
		}
	}
}
