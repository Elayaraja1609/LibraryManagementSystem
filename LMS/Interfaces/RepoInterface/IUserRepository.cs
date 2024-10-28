using LMS.Models;

namespace LMS.Interfaces.RepoInterface
{
	public interface IUserRepository
	{
		Task<User?> GetByIdAsync(int id);
		Task<IEnumerable<User>> GetAllAsync();
		Task AddAsync(User user);
		void Update(User user);
		void Remove(User user);
		Task AddUserLoginAsync(UserLogin user);
	}
}
