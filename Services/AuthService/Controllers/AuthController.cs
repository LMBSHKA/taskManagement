using AuthService.DTOs;
using AuthService.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

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
		public async Task<IActionResult> Registration([FromBody] RegistrationDTO registrationData)
		{
			var tokenList = await _authRepo.Registration(registrationData);
			if (tokenList == null)
				return BadRequest("Registration failed");

			var accessToken = tokenList[0];
			var refreshToken = tokenList[1];

			return Ok(new { accessToken, refreshToken });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
		{
			if (loginData == null)
				return BadRequest("Invalid login data");

			var tokenList = await _authRepo.Login(loginData);
			if (tokenList == null)
				return BadRequest("Invalid login data");

			var accessToken = tokenList[0];
			var refreshToken = tokenList[1];

			return Ok(new { accessToken, refreshToken });
		}

		[HttpGet("me")]
		public async Task<IActionResult> GetUserInfo()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token");
			if (accessToken == null)
				return BadRequest("Invalid token");

			var user = await _authRepo.GetUserInfo(accessToken!);
			if (user == null)
				return BadRequest("User not found");

			return Ok(user);
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] string refreshToken)
		{
			var accessToken = await _authRepo.Refresh(refreshToken);
			if (accessToken == null)
				return Unauthorized();

			return Ok(accessToken);
		}
	}
}
