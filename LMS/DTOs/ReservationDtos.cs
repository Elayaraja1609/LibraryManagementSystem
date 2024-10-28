namespace LMS.DTOs
{
	public class ReservationDtos
	{
		public int Id { get; set; }

		public DateTime ReservationDate { get; set; }

		public DateTime ExpirationDate { get; set; }
		public int? BookId {  get; set; }
		public string? BookName { get; set; }
		public int? UserId { get; set; }
		public string? UserName { get; set; }
		public bool Status { get; set; }
	}
}
