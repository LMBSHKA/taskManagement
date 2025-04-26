using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs
{
	public class RegistrationDTO
	{
		[Required]
		public string? Email { get; set; }
		[Required]
		public string? Password { get; set; }
		[Required]
		public string? Name {  get; set; }
		[Required]
		public string? Surname { get; set; }
	}
}
