namespace LMS.Models
{
	public class BookTransaction
	{
		public int Id { get; set; }

		public DateTime TransactionDate { get; set; }

		public DateTime DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public int BookId { get; set; }

		public int UserId { get; set; }
		public bool status { get; set; } = true;

		public virtual Book? Book { get; set; }

		public virtual User? User { get; set; }
	}
}
