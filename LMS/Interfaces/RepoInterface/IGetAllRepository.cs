using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IGetAllRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
	}
}
