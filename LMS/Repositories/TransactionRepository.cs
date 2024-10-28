using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly LibraryDBContext _context;

		public TransactionRepository(LibraryDBContext context)
		{
			_context = context;
		}

		public async Task<BookTransaction?> GetByIdAsync(int id)
		{
			return await _context.Transactions.FindAsync(id);
		}

		public async Task<IEnumerable<BookTransaction>> GetAllAsync()
		{
			return await _context.Transactions.ToListAsync();
		}

		public async Task AddAsync(BookTransaction trans)
		{
			await _context.Transactions.AddAsync(trans);
			//await _context.SaveChangesAsync();
		}

		public void Update(BookTransaction trans)
		{
			_context.Transactions.Update(trans);
			//_context.SaveChanges();
		}

		public void Remove(BookTransaction trans)
		{
			_context.Transactions.Remove(trans);
			//_context.SaveChanges();
		}

	}
}
