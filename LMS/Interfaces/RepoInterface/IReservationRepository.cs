using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IReservationRepository
	{
		Task<Reservation?> GetByIdAsync(int id);
		Task<IEnumerable<Reservation>> GetAllAsync();
		Task AddAsync(Reservation res);
		void Update(Reservation res);
		void Remove(Reservation res);
	}
}
