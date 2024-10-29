using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface ITransactionRepository
	{
		Task<IEnumerable<BookTransaction>> GetAllAsync();
	}
}
