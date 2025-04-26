using AuthService.Models;

namespace AuthService.Handlers
{
	public interface IPasswordHandler
	{
		string HashPassword(User user, string password);
		bool VerifyPassword(User user, string password);
	}
}
