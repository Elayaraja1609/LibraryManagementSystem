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
			Users1 = new UserRepository(_context);
			Reservations = new ReservationRepository(_context);
			Authors = new AuthorRepository(_context);
			BookTransactions = new TransactionRepository(_context);
			Roles = new RoleRepository(_context);

		}

		public IRepository<Book> Books { get; private set; }
		public IRepository<User> Users { get; private set; }
		public IUserRepository Users1 { get; private set; }
		public IRepository<Reservation> Reservations { get; private set; }
		public IRepository<Author> Authors { get; private set; }
		public IGetAllRepository<BookTransaction> BookTransactions { get; private set; }
		public IGetAllRepository<Role> Roles { get; private set; }
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
