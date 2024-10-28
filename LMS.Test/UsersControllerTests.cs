using LMS.Controllers;
using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Tests.Controllers
{
	[TestFixture]
	public class UsersControllerTests
	{
		private Mock<ITransactionService> _mockTransactionService;
		private Mock<IBookService> _mockBookService;
		private Mock<IUserServices> _mockUserService;
		private UsersController _controller;

		[SetUp]
		public void Setup()
		{
			_mockTransactionService = new Mock<ITransactionService>();
			_mockBookService = new Mock<IBookService>();
			_mockUserService = new Mock<IUserServices>();
			_controller = new UsersController(_mockTransactionService.Object, _mockBookService.Object, _mockUserService.Object);
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsOk_WhenOverdueBooksExist()
		{
			// Arrange
			var currentDate = DateTime.Now;
			List<BookTransaction> overdueBooks = new List<BookTransaction>
			{
				new BookTransaction{ Book=new Book{ BookTitle="Book1"} },
				new BookTransaction{ Book=new Book{ BookTitle="Book2"} }
			};
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ReturnsAsync(overdueBooks);

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>()); // Use Assert.That for type checks
			var okResult = result as OkObjectResult;
			Assert.That(okResult?.Value, Is.EqualTo(overdueBooks)); // Use null conditional operator
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsNotFound_WhenNoOverdueBooksExist()
		{
			// Arrange
			var currentDate = DateTime.Now;
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ReturnsAsync(new List<BookTransaction>());

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate);

			// Assert
			Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
			var notFoundResult = result as NotFoundObjectResult;
			Assert.That(notFoundResult?.Value, Is.EqualTo("No overdue books found.")); // Use null conditional operator
		}

		[Test]
		public async Task BorrowBook_ReturnsOk_WhenBookIsBorrowedSuccessfully()
		{
			// Arrange
			var newTrans = new TransactionDtos { UserId = 1, BookId = 1, TransactionDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
			_mockTransactionService.Setup(s => s.BorrowBookAsync(newTrans.UserId, newTrans.BookId, newTrans.TransactionDate, newTrans.DueDate))
				.ReturnsAsync("Book borrowed successfully.");

			// Act
			var result = await _controller.BorrowBook(newTrans);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult?.Value, Is.EqualTo("Book borrowed successfully.")); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task ReturnBook_ReturnsOk_WhenBookIsReturnedSuccessfully()
		{
			// Arrange
			var newTrans = new TransactionDtos { UserId = 1, BookId = 1, ReturnDate = DateTime.Now };
			_mockTransactionService.Setup(s => s.ReturnBookAsync(newTrans.UserId, newTrans.BookId, (DateTime)newTrans.ReturnDate))
				.ReturnsAsync("Book returned successfully.");

			// Act
			var result = await _controller.ReturnBook(newTrans);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult?.Value, Is.EqualTo("Book returned successfully.")); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task SearchBooks_ReturnsOk_WhenBooksFound()
		{
			// Arrange
			var keyword = "test";
			List<BookDtos> books = new List<BookDtos>
			{
				new BookDtos{Title="Book1"},
				new BookDtos{Title="Book2"}
			};

			_mockBookService.Setup(s => s.SearchBooksAsync(keyword)).ReturnsAsync(books);

			// Act
			var result = await _controller.SearchBooks(keyword);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.That(okResult?.Value, Is.EqualTo(books)); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task SearchBooks_ReturnsNotFound_WhenNoBooksFound()
		{
			// Arrange
			var keyword = "non-existing-book";
			_mockBookService.Setup(s => s.SearchBooksAsync(keyword)).ReturnsAsync(new List<BookDtos>());

			// Act
			var result = await _controller.SearchBooks(keyword);

			// Assert
			Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
			var notFoundResult = result as NotFoundObjectResult;
			Assert.That(notFoundResult?.Value, Is.EqualTo("No books found matching the keyword.")); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task BorrowBook_ReturnsBadRequest_WhenTransactionDetailsNull()
		{
			// Act
			var result = await _controller.BorrowBook(null);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.That(badRequestResult?.Value, Is.EqualTo("Transaction details cannot be null.")); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task ReturnBook_ReturnsBadRequest_WhenTransactionDetailsNull()
		{
			// Act
			var result = await _controller.ReturnBook(null);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.That(badRequestResult?.Value, Is.EqualTo("Transaction details cannot be null.")); // Ensure consistency with your controller logic
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsBadRequest_WhenExceptionThrown()
		{
			// Arrange
			var currentDate = DateTime.Now;
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ThrowsAsync(new LMSException("Service failure"));

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.That(badRequestResult?.Value, Is.EqualTo("Service failure")); // Ensure consistency with your controller logic
		}
	}
}
