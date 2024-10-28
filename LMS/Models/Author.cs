namespace LMS.Models
{
	public class Author
	{
		public int Id { get; set; }

		public string AuthorName { get; set; } = null!;

		public string AuthorSurname { get; set; } = null!;

		public DateTime? AuthorBirthDate { get; set; }

		public string AuthorImg { get; set; } = null!;

		public virtual ICollection<Book> Books { get; set; } = new List<Book>();
	}
}
