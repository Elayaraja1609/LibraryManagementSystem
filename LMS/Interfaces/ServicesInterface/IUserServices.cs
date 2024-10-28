using LMS.DTOs;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IUserServices
	{
		Task<IEnumerable<UserDtos>> GetAllUsersAsync();
		Task<int> AddUserAsync(UserDtos newUser);
		Task<bool> UpdateUserAsync(int id, UserDtos updatedUser);
		Task<bool> DeleteUserAsync(int userId);
	}
}
