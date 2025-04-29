using AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Handlers
{
	public class JWTHandler : IJWTHandler
	{
		private readonly IConfiguration _configuration;

		public JWTHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GetRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		public string GenerateJwtToken(User user)
		{
			var issuer = _configuration["JwtConfig:Issuer"];
			var audience = _configuration["JwtConfig:Audience"];
			var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]!);
			var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
			var tokenExpiry = DateTime.UtcNow.AddMinutes(tokenValidityMins);

			var token = new JwtSecurityToken(issuer, audience,
				[
					new Claim(JwtRegisteredClaimNames.Name, user.Name!),
					new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
				],
				expires: tokenExpiry,
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha512Signature));

			var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

			return accessToken;
		}
	}
}
