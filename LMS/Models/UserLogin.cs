namespace LMS.Models
{
	public class UserLogin
	{
		public int Id { get; set; }
		public DateTime? UserLoginDate { get; set; }
		public virtual User? User { get; set; }
	}
}
