namespace LMS.Models
{
	public class Book
	{
		public int BookId { get; set; }

		public string BookTitle { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public double BookPrice { get; set; }

		public string BookImg { get; set; } = null!;

		public int PageCount { get; set; }

		public DateTime? BookPublishedYear { get; set; }
		public string Publisher { get; set; } = null!;

		public int? AuthorId { get; set; }

		public int? CategoryId { get; set; }
		public int BookQty { get; set; }
		public bool IsAvailable { get; set; }
		public virtual Author? Author { get; set; }

		public virtual Category? Category { get; set; }
		public virtual ICollection<BookTransaction> Transactions { get; set; } = new List<BookTransaction>();

		public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
	}
}
