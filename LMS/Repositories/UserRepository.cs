using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;


namespace LMS.Repositories
{
	public class UserRepository : IRepository<User>, IUserRepository
	{
		private readonly LibraryDBContext _context;

		public UserRepository(LibraryDBContext context)
		{
			_context = context;
		}

		public async Task<User?> GetByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task AddAsync(User user)
		{
			await _context.Users.AddAsync(user);
		}

		public void Update(User user)
		{
			_context.Users.Update(user);
		}

		public void Remove(User user)
		{
			_context.Users.Remove(user);
		}
		public async Task AddUserLoginAsync(UserLogin user)
		{
			await _context.UserLogins.AddAsync(user);
		}
	}
}
