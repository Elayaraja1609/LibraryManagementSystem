using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IRoleRepository
	{
		Task<IEnumerable<Role>> GetAllAsync();
	}
}
