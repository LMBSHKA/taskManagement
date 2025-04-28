using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
	public class RefreshToken
	{
		[Key]
		public Guid Id { get; set; }
		public int UserId { get; set; }
		public string? Token { get; set; }
		public string? Expiry { get; set; }
	}
}
