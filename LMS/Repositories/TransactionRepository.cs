using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class TransactionRepository : IGetAllRepository<BookTransaction>
	{
		private readonly LibraryDBContext _context;

		public TransactionRepository(LibraryDBContext context)
		{
			_context = context;
		}


		public async Task<IEnumerable<BookTransaction>> GetAllAsync()
		{
			return await _context.Transactions.ToListAsync();
		}

	}
}
