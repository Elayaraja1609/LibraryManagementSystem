using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IBookRepository
	{
		Task<Book?> GetByIdAsync(int id);
		Task<IEnumerable<Book>> GetAllAsync();
		Task AddAsync(Book book);
		void Update(Book book);
		void Remove(Book book);
	}
}
