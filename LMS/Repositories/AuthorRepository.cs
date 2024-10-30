using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class AuthorRepository : IRepository<Author>
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
		}

		public void Update(Author author)
		{
			_context.Authors.Update(author);
		}

		public void Remove(Author author)
		{
			_context.Authors.Remove(author);
		}

	}
}
