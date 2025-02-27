﻿using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class ReservationRepository : IRepository<Reservation>
	{
		private readonly LibraryDBContext _context;

		public ReservationRepository(LibraryDBContext context)
		{
			_context = context;
		}

		public async Task<Reservation?> GetByIdAsync(int id)
		{
			return await _context.Reservations.FindAsync(id);
		}

		public async Task<IEnumerable<Reservation>> GetAllAsync()
		{
			return await _context.Reservations.ToListAsync();
		}

		public async Task AddAsync(Reservation res)
		{
			await _context.Reservations.AddAsync(res);
		}

		public void Update(Reservation res)
		{
			_context.Reservations.Update(res);
		}

		public void Remove(Reservation res)
		{
			_context.Reservations.Remove(res);
		}

	}
}
