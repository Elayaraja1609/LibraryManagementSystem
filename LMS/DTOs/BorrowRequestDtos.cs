namespace LMS.DTOs
{
	public class BorrowRequestDtos
	{
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime DueDate { get; set; }
	}
}
