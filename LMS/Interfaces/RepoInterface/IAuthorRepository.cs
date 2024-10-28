using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IAuthorRepository
	{
		Task<Author?> GetByIdAsync(int id);
		Task<IEnumerable<Author>> GetAllAsync();
		Task AddAsync(Author author);
		void Update(Author author);
		void Remove(Author author);
	}
}
