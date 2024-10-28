namespace LMS.DTOs
{
	public class ReturnRequestDtos
	{
		public int UserId { get; set; }
		public int BookId { get; set; }
		public int TransactionId { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime ReturnDate { get; set; }
	}
}
