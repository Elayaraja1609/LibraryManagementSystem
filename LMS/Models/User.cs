namespace LMS.Models
{
	public class User
	{
		public int Id { get; set; }

		public string? Firstname { get; set; } = null!;

		public string? Lastname { get; set; } = null!;

		public string? Username { get; set; } = null!;

		public string? Password { get; set; } = null!;
		public string? Email { get; set; } = null!;
		public string? Contact { get; set; } = null!;
		public string? Address { get; set; } = null!;

		public int? RoleId { get; set; }
		public bool Active { get; set; } = true;
		public virtual ICollection<BookTransaction> Transactions { get; set; } = new List<BookTransaction>();
		public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
		public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

		public virtual Role? Role { get; set; }
	}
}
