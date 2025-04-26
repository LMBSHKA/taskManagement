using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Password { get; set; }
		public string? Email { get; set; }

		public User() { }

		public User(string name, string surname, string password, string email)
		{
			Name = name;
			Surname = surname;
			Password = password;
			Email = email;
		}
	}
}
