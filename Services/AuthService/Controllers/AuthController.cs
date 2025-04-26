using AuthService.DTOs;
using AuthService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDTO loginData)
		{
			if (loginData == null)
				return BadRequest("Invalid login data");

			var tokenList = _authRepo.Login(loginData).Result;
			if (tokenList == null)
				return BadRequest("Invalid login data");

			var accessToken = tokenList[0];
			var refreshToken = tokenList[1];

			return Ok(new { accessToken, refreshToken });
		}
	}
}
