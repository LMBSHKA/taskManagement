using AuthService.Models;

namespace AuthService.Handlers
{
	public interface IJWTHandler
	{
		string GetRefreshToken();
		string GenerateJwtToken(User user);
	}
}
