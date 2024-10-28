using LMS.DTOs;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IAuthorService
	{
		Task<IEnumerable<AuthorDto>> GetAllAuthorAsync();
		Task<int> AddAuthorAsync(AuthorDto newAuthor);
		Task<bool> UpdateAuthorAsync(int id, AuthorDto updatedAuthor);
		Task<bool> DeleteAuthorAsync(int authorId);
	}
}
