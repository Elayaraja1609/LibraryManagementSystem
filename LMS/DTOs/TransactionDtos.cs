namespace LMS.DTOs
{
	public class TransactionDtos
	{
		public int Id { get; set; }

		public DateTime TransactionDate { get; set; }

		public DateTime DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public int BookId { get; set; }
		public string BookName { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public bool status { get; set; } = true;
	}
}
