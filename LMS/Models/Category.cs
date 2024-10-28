namespace LMS.Models
{
	public class Category
	{
		public int Id { get; set; }

		public string CategoryName { get; set; } = null!;

		public virtual ICollection<Book> Books { get; set; } = new List<Book>();
	}
}
