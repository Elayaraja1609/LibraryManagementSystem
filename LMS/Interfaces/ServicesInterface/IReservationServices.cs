using LMS.DTOs;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IReservationServices
	{
		Task<IEnumerable<ReservationDtos>> GetAllReservationAsync();
		Task<int> AddReservationAsync(ReservationDtos updatedReser);
		Task<bool> UpdateReservationAsync(int id, ReservationDtos updatedReserv);
		Task<bool> DeleteReservationAsync(int reservId);
	}
}
