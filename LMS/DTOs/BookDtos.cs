using LMS.Models;

namespace LMS.DTOs
{
	public class BookDtos
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;
		public string ISBN { get; set; } = null!;
		public double Price { get; set; }

		public string Image { get; set; } = null!;

		public int PageCount { get; set; }

		public DateTime? PublishedYear { get; set; }
		public string Publisher { get; set; } = null!;

		public int? AuthorId { get; set; }
		public string? AuthorName { get; set; }

		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public int BookQty { get; set; }
		public bool IsAvailable { get; set; }
	}
}
