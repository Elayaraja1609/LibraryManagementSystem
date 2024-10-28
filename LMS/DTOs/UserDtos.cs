namespace LMS.DTOs
{
	public class UserDtos
	{
		public int Id { get; set; }

		public string? Firstname { get; set; }

		public string? Lastname { get; set; }

		public string? Username { get; set; }

		public string? Password { get; set; }
		public string? Email { get; set; }
		public string? Contact {  get; set; }
		public string? Address { get; set; }

		public int? RoleId { get; set; }
		public bool Active { get; set; }
	}
}
