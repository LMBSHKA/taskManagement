using AuthService.Database;
using AuthService.DTOs;
using AuthService.Handlers;
using AuthService.Models;
using Microsoft.AspNetCore.Authentication;

namespace AuthService.Repositories
{
	public class AuthRepo : IAuthRepo
	{
		private readonly AppDbContext _context;
		private readonly IPasswordHandler _passwordHandler;
		private readonly IJWTHandler _jwtHandler;
		private readonly IConfiguration _configuration;
		public AuthRepo(AppDbContext context, IPasswordHandler passwordHandler, IJWTHandler jwtHandler, 
			IConfiguration config)
		{
			_passwordHandler = passwordHandler;
			_context = context;
			_jwtHandler = jwtHandler;
			_configuration = config;
 		}

		public async Task<List<string>> Registration(RegistrationDTO registrationData)
		{
			if (registrationData == null)
				return null!;

			var newUser = new User(
				registrationData.Name!,
				registrationData.Surname!,
				string.Empty,
				registrationData.Email!);

			newUser.Password = _passwordHandler.HashPassword(newUser, registrationData.Password!);

			try
			{
				await _context.Users.AddAsync(newUser);
				var token = _jwtHandler.GenerateJwtToken(newUser);
				await _context.SaveChangesAsync();

				var refreshToken = CreateRefreshToken(newUser).Result;

				return [token, refreshToken];
			}

			catch
			{
				return null!;
			}
		}

		private async Task<string> CreateRefreshToken(User user)
		{
			var refreshToken = _jwtHandler.GetRefreshToken();
			var refreshTokenValidity = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");
			var expiry = DateTime.UtcNow.AddDays(refreshTokenValidity);

			await _context.RefreshTokens.AddAsync(new RefreshToken
			{

				Token = refreshToken,
				Expiry = expiry,
				UserId = user.Id,
			});
			await _context.SaveChangesAsync();

			return refreshToken;
		}
	}
}
