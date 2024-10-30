using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IUserRepository
	{
		Task AddUserLoginAsync(UserLogin user);
	}
}
