using AuthService.DTOs;

namespace AuthService.Repositories
{
	public interface IAuthRepo
	{
		Task<List<string>> Registration(RegistrationDTO registrationData);
		Task<List<string>> Login(LoginDTO loginData);
		Task<UserInfoDTO> GetUserInfo(string accesToken);
		Task<string> Refresh(string refreshToken);
	}
}
