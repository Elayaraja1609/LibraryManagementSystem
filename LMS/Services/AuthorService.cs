using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;

namespace LMS.Services
{
	public class AuthorService: IAuthorService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AuthorService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<AuthorDto>> GetAllAuthorAsync()
		{
			var authors = await _unitOfWork.Authors.GetAllAsync();
			return authors.Select(a => new AuthorDto
			{
				Id=a.Id,
				FirstName=a.AuthorName,
				LastName=a.AuthorSurname,
				DOB=a.AuthorBirthDate,
				Image=a.AuthorImg
			});
		}
		public async Task<int> AddAuthorAsync(AuthorDto newAuthor)
		{

			var author = new Author
			{
				Id= newAuthor.Id,
				AuthorName=newAuthor.FirstName,
				AuthorSurname=newAuthor.LastName,
				AuthorBirthDate=newAuthor.DOB,
				AuthorImg=newAuthor.Image
			};
			_ = _unitOfWork.Authors.AddAsync(author);
			await _unitOfWork.CompleteAsync();
			return author.Id;
		}
		public async Task<bool> UpdateAuthorAsync(int id, AuthorDto updatedAuthor)
		{
			var author = await _unitOfWork.Authors.GetByIdAsync(id);
			if (author == null)
			{
				return false;
			}

			author.AuthorName = updatedAuthor.FirstName;
			author.AuthorSurname = updatedAuthor.LastName;
			author.AuthorBirthDate = updatedAuthor.DOB;
			author.AuthorImg = updatedAuthor.Image;


			await _unitOfWork.CompleteAsync();

			return true;
		}
		public async Task<bool> DeleteAuthorAsync(int authorId)
		{
			var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
			if (author == null) return false;

			_unitOfWork.Authors.Remove(author);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
