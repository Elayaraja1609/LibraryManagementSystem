using NUnit.Framework;
using Moq;
using LMS.Controllers;
using LMS.Interfaces.ServicesInterface;
using LMS.DTOs;
using LMS.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LMS.Tests
{
	[TestFixture]
	public class AdminControllerTests
	{
		private Mock<IAuthorService> _mockAuthorService;
		private Mock<IBookService> _mockBookService;
		private Mock<IUserServices> _mockUserService;
		private AdminController _controller;

		[SetUp]
		public void SetUp()
		{
			_mockAuthorService = new Mock<IAuthorService>();
			_mockBookService = new Mock<IBookService>();
			_mockUserService = new Mock<IUserServices>();

			_controller = new AdminController(_mockAuthorService.Object, _mockBookService.Object, _mockUserService.Object);
		}

		#region Author Tests

		[Test]
		public async Task GetAuthors_ShouldReturnOkResult_WhenAuthorsExist()
		{
			// Arrange
			var authors = new List<AuthorDto> { new AuthorDto { Id = 1, FirstName = "Author 1" } };
			_mockAuthorService.Setup(s => s.GetAllAuthorAsync()).ReturnsAsync(authors);

			// Act
			var result = await _controller.GetAuthors();

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult.Value, Is.EqualTo(authors));
		}

		[Test]
		public async Task AddAuthor_ShouldReturnCreatedAtActionResult_WhenAuthorIsAdded()
		{
			// Arrange
			var newAuthor = new AuthorDto { FirstName = "New Author" };
			_mockAuthorService.Setup(s => s.AddAuthorAsync(newAuthor)).ReturnsAsync(1);

			// Act
			var result = await _controller.AddAuthor(newAuthor);

			// Assert
			Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
			var createdResult = result as CreatedAtActionResult;
			Assert.That(createdResult, Is.Not.Null);
			Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetAuthors)));
		}

		[Test]
		public async Task UpdateAuthor_ShouldReturnBadRequest_WhenAuthorIsNotFound()
		{
			// Arrange
			var updatedAuthor = new AuthorDto { FirstName = "Updated Author" };
			_mockAuthorService.Setup(s => s.UpdateAuthorAsync(1, updatedAuthor)).ReturnsAsync(false);

			// Act
			var result = await _controller.UpdateAuthor(1, updatedAuthor);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
		}

		#endregion

		#region Book Tests

		[Test]
		public async Task GetBooks_ShouldReturnOkResult_WhenBooksExist()
		{
			// Arrange
			var books = new List<BookDtos> { new BookDtos { Id = 1, Title = "Book 1" } };
			_mockBookService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(books);

			// Act
			var result = await _controller.GetBooks();

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult.Value, Is.EqualTo(books));
		}

		[Test]
		public async Task AddBook_ShouldReturnCreatedAtActionResult_WhenBookIsAdded()
		{
			// Arrange
			var newBook = new BookDtos { Title = "New Book" };
			_mockBookService.Setup(s => s.AddBookAsync(newBook)).ReturnsAsync(1);

			// Act
			var result = await _controller.AddBook(newBook);

			// Assert
			Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
			var createdResult = result as CreatedAtActionResult;
			Assert.That(createdResult, Is.Not.Null);
			Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetBooks)));
		}

		[Test]
		public async Task DeleteBook_ShouldReturnOkResult_WhenBookIsDeleted()
		{
			// Arrange
			_mockBookService.Setup(s => s.DeleteBookAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _controller.deleteBook(1);

			// Assert
			Assert.That(result, Is.InstanceOf<OkResult>());
		}

		#endregion

		#region User Tests

		[Test]
		public async Task GetAll_ShouldReturnOkResult_WhenUsersExist()
		{
			// Arrange
			var users = new List<UserDtos> { new UserDtos { Id = 1, Username = "User 1" } };
			_mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

			// Act
			var result = await _controller.GetAll();

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult.Value, Is.EqualTo(users));
		}

		[Test]
		public async Task AddUser_ShouldReturnCreatedAtActionResult_WhenUserIsAdded()
		{
			// Arrange
			var newUser = new UserDtos { Username = "New User" };
			_mockUserService.Setup(s => s.AddUserAsync(newUser)).ReturnsAsync(1);

			// Act
			var result = await _controller.AddUser(newUser);

			// Assert
			Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
			var createdResult = result as CreatedAtActionResult;
			Assert.That(createdResult, Is.Not.Null);
			Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.GetAll)));
		}

		[Test]
		public async Task DeleteUser_ShouldReturnOkResult_WhenUserIsDeleted()
		{
			// Arrange
			_mockUserService.Setup(s => s.DeleteUserAsync(1)).ReturnsAsync(true);

			// Act
			var result = await _controller.Delete(1);

			// Assert
			Assert.That(result, Is.InstanceOf<OkResult>());
		}

		#endregion
	}
}
