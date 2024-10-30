using LMS.Data;
using LMS.Interfaces.RepoInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories
{
	public class RoleRepository: IGetAllRepository<Role>
	{
		private readonly LibraryDBContext _context;

		public RoleRepository(LibraryDBContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Role>> GetAllAsync()
		{
			return await _context.Roles.ToListAsync();
		}
	}
}
