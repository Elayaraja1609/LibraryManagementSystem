using LMS.Interfaces.RepoInterface;
using LMS.Models;
using LMS.Repositories;

namespace LMS.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Book> Books { get; }
		IRepository<User> Users { get; }
		IUserRepository Users1 { get; }
		IGetAllRepository<Role> Roles { get; }
		IRepository<Reservation> Reservations { get; }
		IRepository<Author> Authors { get; }
		IGetAllRepository<BookTransaction> BookTransactions { get; }
		Task<int> CompleteAsync();
	}

	
}
