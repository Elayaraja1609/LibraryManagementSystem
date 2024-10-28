using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class AuthorRepository : IAuthorRepository
	{
		private readonly LibraryDBContext _context;

		public AuthorRepository(LibraryDBContext context)
		{
			_context = context;
		}

		public async Task<Author?> GetByIdAsync(int id)
		{
			return await _context.Authors.FindAsync(id);
		}

		public async Task<IEnumerable<Author>> GetAllAsync()
		{
			return await _context.Authors.ToListAsync();
		}

		public async Task AddAsync(Author author)
		{

			await _context.Authors.AddAsync(author);
			//await _context.SaveChangesAsync();
		}

		public void Update(Author author)
		{
			_context.Authors.Update(author);
			//_context.SaveChanges();
		}

		public void Remove(Author author)
		{
			_context.Authors.Remove(author);
			//_context.SaveChanges();
		}

	}
}
