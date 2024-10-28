using LMS.Interfaces.RepoInterface;
using LMS.Models;
using LMS.Repositories;

namespace LMS.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IBookRepository Books { get; }
		IUserRepository Users { get; }
		IRoleRepository Roles { get; }
		IReservationRepository Reservations { get; }
		IAuthorRepository Authors { get; }
		ITransactionRepository BookTransactions { get; }
		Task<int> CompleteAsync();
	}

	
}
