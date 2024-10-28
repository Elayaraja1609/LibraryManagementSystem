using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services
{
	public class BookService : IBookService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BookService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<BookDtos>> GetAllBooksAsync()
		{
			var books = await _unitOfWork.Books.GetAllAsync();
			return books.Select(b => new BookDtos { 
				Id=b.BookId,
				Title=b.BookTitle,
				Image=b.BookImg,
				PageCount=b.PageCount,
				Price=b.BookPrice,
				AuthorName=b.Author != null?b.Author.AuthorName:b.AuthorId.ToString(),
				PublishedYear=b.BookPublishedYear,
				Publisher = b.Publisher,
				BookQty=b.BookQty,
				IsAvailable=b.IsAvailable
			});
		}
		public async Task<int> AddBookAsync(BookDtos newBook)
		{
			var book = new Book
			{
				BookTitle = newBook.Title,
				BookPrice = newBook.Price,
				ISBN = newBook.ISBN,
				BookImg = newBook.Image,
				PageCount = newBook.PageCount,
				BookPublishedYear = newBook.PublishedYear,
				Publisher = newBook.Publisher,
				BookQty = newBook.BookQty,
				AuthorId = newBook.AuthorId,
				CategoryId = newBook.CategoryId,
				IsAvailable = newBook.IsAvailable
			};
			_ = _unitOfWork.Books.AddAsync(book);
			await _unitOfWork.CompleteAsync();
			return book.BookId;
		}
		public async Task<bool> UpdateBookAsync(int id, BookDtos updatedBook)
		{
			var book = await _unitOfWork.Books.GetByIdAsync(id);
			if (book == null)
			{
				return false;
			}

			book.BookTitle = updatedBook.Title;
			book.ISBN = updatedBook.ISBN;
			book.BookPrice = updatedBook.Price;
			book.BookImg = updatedBook.Image;
			book.PageCount = updatedBook.PageCount;
			book.BookPublishedYear = updatedBook.PublishedYear.HasValue ? updatedBook.PublishedYear.Value : null;
			book.Publisher = updatedBook.Publisher;
			book.BookQty = updatedBook.BookQty;
			book.IsAvailable = updatedBook.IsAvailable;
			book.AuthorId = updatedBook.AuthorId;
			book.CategoryId = updatedBook.CategoryId;


			await _unitOfWork.CompleteAsync();

			return true;
		}
		public async Task<bool> DeleteBookAsync(int bookId)
		{
			var book = await _unitOfWork.Books.GetByIdAsync(bookId);
			if (book == null) return false;

			_unitOfWork.Books.Remove(book);
			await _unitOfWork.CompleteAsync();
			return true;
		}
		public async Task<Book> GetBookByIdAsync(int bookId)
		{
			var book = await _unitOfWork.Books.GetByIdAsync(bookId);
			return book;
		}
		public async Task<IEnumerable<BookDtos>> SearchBooksAsync(string keyword)
		{
			var books = await _unitOfWork.Books.GetAllAsync();
			return books.Where(book => book.BookTitle.Contains(keyword, StringComparison.OrdinalIgnoreCase))
				.Select(b => new BookDtos {
				Id = b.BookId,
				Title = b.BookTitle,
				Image = b.BookImg,
				PageCount = b.PageCount,
				Price = b.BookPrice,
				AuthorName = b.Author != null ? b.Author.AuthorName : b.AuthorId.ToString(),
				PublishedYear = b.BookPublishedYear,
				Publisher = b.Publisher,
				BookQty = b.BookQty,
				IsAvailable = b.IsAvailable
			});
		}
	}
}
