﻿using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly LibraryDBContext _context;

		public BookRepository(LibraryDBContext context)
		{
			_context = context;
		}

		public async Task<Book?> GetByIdAsync(int id)
		{
			return await _context.Books.FindAsync(id);
		}

		public async Task<IEnumerable<Book>> GetAllAsync()
		{
			return await _context.Books.ToListAsync();
		}

		public async Task AddAsync(Book book)
		{
			await _context.Books.AddAsync(book);
			//await _context.SaveChangesAsync();
		}

		public void Update(Book book)
		{
			_context.Books.Update(book);
			//_context.SaveChanges();
		}

		public void Remove(Book book)
		{
			_context.Books.Remove(book);
			//_context.SaveChanges();
		}

	}
}
