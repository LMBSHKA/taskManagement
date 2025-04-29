using AuthService.Database;
using AuthService.DTOs;
using AuthService.Handlers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Repositories
{
	public class AuthRepo : IAuthRepo
	{
		private readonly AppDbContext _context;
		private readonly IPasswordHandler _passwordHandler;
		private readonly IJWTHandler _jwtHandler;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AuthRepo> _logger;
		public AuthRepo(AppDbContext context, IPasswordHandler passwordHandler, IJWTHandler jwtHandler, 
			IConfiguration config, ILogger<AuthRepo> logger)
		{
			_logger = logger;
			_passwordHandler = passwordHandler;
			_context = context;
			_jwtHandler = jwtHandler;
			_configuration = config;
 		}

		public async Task<List<string>> Registration(RegistrationDTO registrationData)
		{
			if (registrationData == null)
				return null!;

			if (await _context.Users.FirstOrDefaultAsync(x => x.Email == registrationData.Email) != null)
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

				var refreshToken = await CreateRefreshToken(newUser);

				_logger.LogInformation($"Registration user, login: {registrationData.Email} " +
					$"name: {registrationData.Name}, surname: {registrationData.Surname}");

				return [token, refreshToken];
			}

			catch
			{
				_logger.LogWarning($"Registration failed, login: {registrationData.Email} " +
					$"name: {registrationData.Name}, surname: {registrationData.Surname}");
				return null!;
			}
		}

		private async Task<string> CreateRefreshToken(User user)
		{
			var refreshTokenData = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);
			var refreshTokenValidity = _configuration.GetValue<int>("JwtConfig:RefreshTokenValidityDays");
			var expiry = DateTime.UtcNow.AddDays(refreshTokenValidity);
			if (refreshTokenData == null)
			{
				var refreshToken = _jwtHandler.GetRefreshToken();

				await _context.RefreshTokens.AddAsync(new RefreshToken
				{

					Token = refreshToken,
					Expiry = expiry.ToString("dd.MM.yyyy"),
					UserId = user.Id,
				});
				await _context.SaveChangesAsync();

				return refreshToken;
			}

			return await UpdateRefreshToken(refreshTokenData, expiry);
		}

		private async Task<string> UpdateRefreshToken(RefreshToken refreshTokenData, DateTime expiry)
		{
			var newRefreshToken = _jwtHandler.GetRefreshToken();
			refreshTokenData.Expiry = expiry.ToString("dd.MM.yyyy");
			refreshTokenData.Token = newRefreshToken;
			_context.RefreshTokens.Update(refreshTokenData);
			await _context.SaveChangesAsync();

			return newRefreshToken;
		}

		public async Task<List<string>> Login(LoginDTO loginData)
		{
			if (loginData.Email == null)
				return null!;

			var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginData.Email);

			if (user == null)
				return null!;

			if (!_passwordHandler.VerifyPassword(user, loginData.Password))
			{
				_logger.LogInformation($"incorrect password, user login: {user.Email}");
				return null!;
			}

			var token = _jwtHandler.GenerateJwtToken(user);
			var refreshToken = await CreateRefreshToken(user);

			return [token, refreshToken];
		}

		public async Task<UserInfoDTO> GetUserInfo(string accessToken)
		{
			var userId = DecodeToken(accessToken);
			if (userId == 0)
				return null!;

			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
			if (user == null)
			{
				_logger.LogInformation($"access token with non-existent id: {userId},\ntoken: {accessToken}");
				return null!;
			}

			return new UserInfoDTO(user.Name, user.Surname, user.Email);
		}

		private int DecodeToken(string accessToken)
		{
			var handler = new JwtSecurityTokenHandler();
			var readToken = handler.ReadToken(accessToken);
			var decodedToken = readToken as JwtSecurityToken;
			if (decodedToken == null)
				return 0;

			var stringUserId = decodedToken.Claims.First(claim => claim.Type == "sub").Value;
			if (stringUserId == null)
				return 0;

			if (!int.TryParse(stringUserId, out int intUserId))
				return 0;

			return intUserId;
		}

		public async Task<string> Refresh(string refreshToken)
		{
			var refreshData = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
			if (refreshData == null || Convert.ToDateTime(refreshData.Expiry) < DateTime.UtcNow)
				return null!;

			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == refreshData.UserId);
			if (user == null)
			{
				return null!;
			}

			var accessToken = _jwtHandler.GenerateJwtToken(user);

			return accessToken;
		}
	}
}