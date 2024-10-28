using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using System.Net;

namespace LMS.Services
{
	public class ReservationServices: IReservationServices
	{
		private readonly IUnitOfWork _unitOfWork;
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
		public ReservationServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IEnumerable<ReservationDtos>> GetAllReservationAsync()
		{
			var reserv = await _unitOfWork.Reservations.GetAllAsync();
			return reserv.Select(a => new ReservationDtos
			{
				Id = a.Id,
				ReservationDate=a.ReservationDate,
				ExpirationDate = a.ExpirationDate,
				BookId = a.BookId,
				UserId = a.UserId,
				Status = a.Status
			});
		}
		public async Task<int> AddReservationAsync(ReservationDtos updatedReser)
		{
			var reserv = new Reservation
			{
				Id= updatedReser.Id,
				ReservationDate = updatedReser.ReservationDate,
				ExpirationDate= updatedReser.ExpirationDate,
				BookId = updatedReser.BookId,
				UserId = updatedReser.UserId,
				Status = updatedReser.Status

			};
			_ = _unitOfWork.Reservations.AddAsync(reserv);
			await _unitOfWork.CompleteAsync();
			return reserv.Id;
		}
		public async Task<bool> UpdateReservationAsync(int id, ReservationDtos updatedReserv)
		{
			var reserv = await _unitOfWork.Reservations.GetByIdAsync(id);
			if (reserv == null)
			{
				return false;
			}

			reserv.Id = updatedReserv.Id;
			reserv.ReservationDate = updatedReserv.ReservationDate;
			reserv.ExpirationDate = updatedReserv.ExpirationDate;
			reserv.BookId = updatedReserv.BookId;
			reserv.UserId = updatedReserv.UserId;
			reserv.Status = updatedReserv.Status;


			await _unitOfWork.CompleteAsync();

			return true;
		}
		public async Task<bool> DeleteReservationAsync(int reservId)
		{
			var reserv = await _unitOfWork.Reservations.GetByIdAsync(reservId);
			if (reserv == null) return false;

			_unitOfWork.Reservations.Remove(reserv);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
