using LMS.DTOs;
using LMS.Models;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IAuthService
	{
		Task<int?> LoginAsync(LoginDto dto);
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<string?> GenerateJwtToken(int userId, int? roleId);
	}
}
