using LMS.Data;
using LMS.Interfaces;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using System.Threading.Tasks;

namespace LMS.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private static readonly SemaphoreSlim _dbSemaphore = new SemaphoreSlim(1, 1);
		private readonly LibraryDBContext _context;

		public UnitOfWork(LibraryDBContext context)
		{
			_context = context;
			Books = new BookRepository(_context);
			Users = new UserRepository(_context);
			Reservations = new ReservationRepository(_context);
			Authors = new AuthorRepository(_context);
			BookTransactions = new TransactionRepository(_context);
			Roles = new RoleRepository(_context);

		}

		public IBookRepository Books { get; private set; }
		public IUserRepository Users { get; private set; }
		public IReservationRepository Reservations { get; private set; }
		public IAuthorRepository Authors { get; private set; }
		public ITransactionRepository BookTransactions { get; private set; }
		public IRoleRepository Roles { get; private set; }
		public async Task<int> CompleteAsync()
		{
			await _dbSemaphore.WaitAsync();
			try
			{
				return await _context.SaveChangesAsync();
			}
			finally
			{
				_dbSemaphore.Release();
			}
		}
		public void Dispose() => _context.Dispose();
	}
}
