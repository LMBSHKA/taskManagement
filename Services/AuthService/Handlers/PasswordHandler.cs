using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace AuthService.Handlers
{
	public class PasswordHandler : IPasswordHandler
	{
		public string HashPassword(User user, string password)
		{
			var passwordHasher = new PasswordHasher<User>();
			var hashedPassword = passwordHasher.HashPassword(user, password);
			
			return hashedPassword;
		}

		public bool VerifyPassword(User user, string password)
		{
			var e = new PasswordHasher<User>();
			if (e.VerifyHashedPassword(user, user.Password!, password) == PasswordVerificationResult.Success)
				return true;

			return false;
		}
	}
}
