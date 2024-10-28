using LMS.DTOs;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IBookService
	{
		Task<IEnumerable<BookDtos>> GetAllBooksAsync();
		Task<int> AddBookAsync(BookDtos newBook);
		Task<bool> UpdateBookAsync(int id, BookDtos updatedBook);
		Task<bool> DeleteBookAsync(int bookId);
		Task<IEnumerable<BookDtos>> SearchBooksAsync(string keyword);
	}
}
