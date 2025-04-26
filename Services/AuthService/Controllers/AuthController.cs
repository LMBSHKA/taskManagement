using AuthService.DTOs;
using AuthService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
	[ApiController]
	[Route("api/auth/")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepo _authRepo;

		public AuthController(IAuthRepo authRepo)
		{
			_authRepo = authRepo;
		}

		[HttpPost("register")]
		public IActionResult Registration([FromBody] RegistrationDTO registrationData)
		{
			var tokenList = _authRepo.Registration(registrationData).Result;
			if (tokenList == null)
				return BadRequest("Registration failed");

			var accessToken = tokenList[0];
			var refreshToken = tokenList[1];

			return Ok(new { accessToken, refreshToken });
		}
	}
}
