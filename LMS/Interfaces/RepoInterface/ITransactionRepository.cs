using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface ITransactionRepository
	{
		Task<BookTransaction?> GetByIdAsync(int id);
		Task<IEnumerable<BookTransaction>> GetAllAsync();
		Task AddAsync(BookTransaction trans);
		void Update(BookTransaction trans);
		void Remove(BookTransaction trans);
	}
}
