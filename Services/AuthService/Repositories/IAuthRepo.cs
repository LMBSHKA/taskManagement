using AuthService.DTOs;

namespace AuthService.Repositories
{
	public interface IAuthRepo
	{
		Task<List<string>> Registration(RegistrationDTO registrationData);
	}
}
