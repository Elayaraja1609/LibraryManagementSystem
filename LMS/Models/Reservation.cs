namespace LMS.Models
{
	public class Reservation
	{
		public int Id { get; set; }

		public DateTime ReservationDate { get; set; }

		public DateTime ExpirationDate { get; set; }

		public int? BookId { get; set; }

		public int? UserId { get; set; }
		public bool Status { get; set; }

		public virtual Book? Book { get; set; }

		public virtual User? User { get; set; }
	}
}
