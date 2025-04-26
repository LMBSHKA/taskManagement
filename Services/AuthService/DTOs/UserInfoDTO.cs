namespace AuthService.DTOs
{
	public class UserInfoDTO
	{
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string? Email { get; set; }

		public UserInfoDTO() { }

		public UserInfoDTO(string? name, string? surname, string? email)
		{
			Name = name;
			Surname = surname;
			Email = email;
		}
	}
}
