using LMS.Controllers;
using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace LMS.Tests.Controllers
{
	[TestFixture]
	public class UsersControllerTests
	{
		private Mock<ITransactionService> _mockTransactionService;
		private Mock<IBookService> _mockBookService;
		private UsersController _controller;

		[SetUp]
		public void SetUp()
		{
			_mockTransactionService = new Mock<ITransactionService>();
			_mockBookService = new Mock<IBookService>();
			_controller = new UsersController(_mockTransactionService.Object, _mockBookService.Object);
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsOk_WhenOverdueBooksFound()
		{
			// Arrange
			var currentDate = DateTime.Now;
			var overdueBooks = new List<BookTransaction> { new BookTransaction { Id = 1, BookId = 1, UserId = 1 } };
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ReturnsAsync(overdueBooks);

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate) as OkObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
			Assert.AreEqual(overdueBooks, result.Value);
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsNotFound_WhenNoOverdueBooksFound()
		{
			// Arrange
			var currentDate = DateTime.Now;
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ReturnsAsync(new List<BookTransaction>());

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate) as NotFoundObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
			Assert.AreEqual("No overdue books found.", result.Value);
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsBadRequest_WhenLMSExceptionThrown()
		{
			// Arrange
			var currentDate = DateTime.Now;
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ThrowsAsync(new LMSException("Error retrieving overdue books"));

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate) as BadRequestObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
			Assert.AreEqual("Error retrieving overdue books", result.Value);
		}

		[Test]
		public async Task CheckOverdueBooks_ReturnsInternalServerError_WhenUnexpectedExceptionThrown()
		{
			// Arrange
			var currentDate = DateTime.Now;
			_mockTransactionService.Setup(s => s.CheckOverdueBooksAsync(currentDate)).ThrowsAsync(new Exception("Unexpected error"));

			// Act
			var result = await _controller.CheckOverdueBooks(currentDate) as ObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
			Assert.AreEqual("Failed to check overdue: Unexpected error", result.Value);
		}

		[Test]
		public async Task BorrowBooks_ReturnsResults_WhenBorrowRequestsAreProcessed()
		{
			// Arrange
			var borrowRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 1, TransactionDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) }
			};
			_mockTransactionService.Setup(s => s.BorrowBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.ReturnsAsync("Borrowed successfully");

			// Act
			var result = await _controller.BorrowBooks(borrowRequests) as string[];

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("Borrowed successfully", result[0]);
		}

		[Test]
		public async Task ReturnBooks_ReturnsResults_WhenReturnRequestsAreProcessed()
		{
			// Arrange
			var returnRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 1, ReturnDate = DateTime.Now }
			};
			_mockTransactionService.Setup(s => s.ReturnBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
				.ReturnsAsync("Returned successfully");

			// Act
			var result = await _controller.ReturnBooks(returnRequests) as string[];

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("Returned successfully", result[0]);
		}

		[Test]
		public async Task SearchBooks_ReturnsOk_WhenBooksFound()
		{
			// Arrange
			var keyword = "test";
			var books = new List<BookDtos> { new BookDtos { Id = 1, Title = "Test Book" } };
			_mockBookService.Setup(s => s.SearchBooksAsync(keyword)).ReturnsAsync(books);

			// Act
			var result = await _controller.SearchBooks(keyword) as OkObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
			Assert.AreEqual(books, result.Value);
		}

		[Test]
		public async Task SearchBooks_ReturnsNotFound_WhenNoBooksFound()
		{
			// Arrange
			var keyword = "non-existent";
			_mockBookService.Setup(s => s.SearchBooksAsync(keyword)).ReturnsAsync(new List<BookDtos>());

			// Act
			var result = await _controller.SearchBooks(keyword) as NotFoundObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
			Assert.AreEqual("No books found matching the keyword.", result.Value);
		}

		[Test]
		public async Task SearchBooks_ReturnsBadRequest_WhenKeywordIsNullOrEmpty()
		{
			// Arrange
			string keyword = null;

			// Act
			var result = await _controller.SearchBooks(keyword) as BadRequestObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
			Assert.AreEqual("Search keyword cannot be null or empty.", result.Value);
		}

		[Test]
		public async Task SearchBooks_ReturnsInternalServerError_WhenUnexpectedExceptionThrown()
		{
			// Arrange
			var keyword = "test";
			_mockBookService.Setup(s => s.SearchBooksAsync(keyword)).ThrowsAsync(new Exception("Unexpected error"));

			// Act
			var result = await _controller.SearchBooks(keyword) as ObjectResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
			Assert.AreEqual("Failed to search for books: Unexpected error", result.Value);
		}
	}
}
