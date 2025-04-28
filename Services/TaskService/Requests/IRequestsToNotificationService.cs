using TaskService.DTOs;
using TaskService.Models;

namespace TaskService.Requests
{
	public interface IRequestsToNotificationService
	{
		Task RequestToCreateNotification(Job job, TypeNotification type);
	}
}
