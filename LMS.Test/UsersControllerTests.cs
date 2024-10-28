using LMS.Controllers;
using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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
		private UsersController _controller;
		private Mock<ITransactionService> _mockTransactionService;
		private Mock<IBookService> _mockBookService;
		private Mock<IUserServices> _mockUserService;

		[SetUp]
		public void SetUp()
		{
			_mockTransactionService = new Mock<ITransactionService>();
			_mockBookService = new Mock<IBookService>();
			_mockUserService = new Mock<IUserServices>();

			_controller = new UsersController(
				_mockTransactionService.Object,
				_mockBookService.Object,
				_mockUserService.Object
			);
		}

		[Test]
		public async Task BorrowBooks_ReturnsOk_WhenAllBooksBorrowedSuccessfully()
		{
			// Arrange
			var borrowRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 101, TransactionDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) },
				new TransactionDtos { UserId = 2, BookId = 102, TransactionDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) }
			};

			_mockTransactionService.Setup(s => s.BorrowBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.ReturnsAsync("Book borrowed successfully.");

			// Act
			var result = await _controller.BorrowBooks(borrowRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.AreEqual(200, okResult.StatusCode);
			var responseMessages = okResult.Value as string[];
			Assert.AreEqual(2, responseMessages.Length);
			Assert.IsTrue(responseMessages.All(msg => msg == "Book borrowed successfully."));
		}

		[Test]
		public async Task BorrowBooks_ReturnsBadRequest_WhenRequestsAreEmpty()
		{
			// Arrange
			var borrowRequests = new List<TransactionDtos>();

			// Act
			var result = await _controller.BorrowBooks(borrowRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.AreEqual(400, badRequestResult.StatusCode);
			Assert.AreEqual("Transaction details cannot be null or empty.", badRequestResult.Value);
		}

		[Test]
		public async Task ReturnBooks_ReturnsOk_WhenAllBooksReturnedSuccessfully()
		{
			// Arrange
			var returnRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 101, ReturnDate = DateTime.Now },
				new TransactionDtos { UserId = 2, BookId = 102, ReturnDate = DateTime.Now }
			};

			_mockTransactionService.Setup(s => s.ReturnBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
				.ReturnsAsync("Book returned successfully.");

			// Act
			var result = await _controller.ReturnBooks(returnRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<OkObjectResult>());
			var okResult = result as OkObjectResult;
			Assert.AreEqual(200, okResult.StatusCode);
			var responseMessages = okResult.Value as string[];
			Assert.AreEqual(2, responseMessages.Length);
			Assert.IsTrue(responseMessages.All(msg => msg == "Book returned successfully."));
		}

		[Test]
		public async Task ReturnBooks_ReturnsBadRequest_WhenRequestsAreEmpty()
		{
			// Arrange
			var returnRequests = new List<TransactionDtos>();

			// Act
			var result = await _controller.ReturnBooks(returnRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.AreEqual(400, badRequestResult.StatusCode);
			Assert.AreEqual("Transaction details cannot be null or empty.", badRequestResult.Value);
		}

		[Test]
		public async Task BorrowBooks_ReturnsBadRequest_WhenServiceThrowsLMSException()
		{
			// Arrange
			var borrowRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 101, TransactionDate = DateTime.Now, DueDate = DateTime.Now.AddDays(14) }
			};

			_mockTransactionService.Setup(s => s.BorrowBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.ThrowsAsync(new LMSException("Borrowing error."));

			// Act
			var result = await _controller.BorrowBooks(borrowRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.AreEqual(400, badRequestResult.StatusCode);
			Assert.AreEqual("Borrowing error.", badRequestResult.Value);
		}

		[Test]
		public async Task ReturnBooks_ReturnsBadRequest_WhenServiceThrowsLMSException()
		{
			// Arrange
			var returnRequests = new List<TransactionDtos>
			{
				new TransactionDtos { UserId = 1, BookId = 101, ReturnDate = DateTime.Now }
			};

			_mockTransactionService.Setup(s => s.ReturnBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
				.ThrowsAsync(new LMSException("Returning error."));

			// Act
			var result = await _controller.ReturnBooks(returnRequests);

			// Assert
			Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
			var badRequestResult = result as BadRequestObjectResult;
			Assert.AreEqual(400, badRequestResult.StatusCode);
			Assert.AreEqual("Returning error.", badRequestResult.Value);
		}
	}
}
