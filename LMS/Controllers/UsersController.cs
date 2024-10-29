using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace LMS.Controllers
{
	[Route("v1/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ITransactionService _bookTransService;
		private readonly IBookService _bookService;

		public UsersController(ITransactionService bookTransService, IBookService bookService)
		{
			_bookTransService = bookTransService;
			_bookService = bookService;
		}

		[HttpGet("overdue-books/{currentDate}")]
		public async Task<IActionResult> CheckOverdueBooks([FromRoute] DateTime currentDate)
		{
			try
			{
				var overdueBooks = await _bookTransService.CheckOverdueBooksAsync(currentDate);
				if (overdueBooks == null || !overdueBooks.Any())
				{
					return NotFound("No overdue books found.");
				}

				return Ok(overdueBooks);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to check overdue: " + ex.Message);
			}
		}

		[HttpPost("borrow-books")]
		public async Task<string[]> BorrowBooks([FromBody] List<TransactionDtos> borrowRequests)
		{
			var results = new ConcurrentBag<string>();

			// Run each borrowing operation in parallel
			Parallel.ForEach(borrowRequests, request =>
			{
				try
				{
					// Call BorrowBookAsync in parallel
					var result = _bookTransService.BorrowBookAsync(request.UserId, request.BookId, request.TransactionDate, request.DueDate).Result;
					results.Add(result);
				}
				catch (Exception ex)
				{
					results.Add($"Failed to borrow book with ID {request.BookId} for user {request.UserId}: {ex.Message}");
				}
			});

			return results.ToArray();
		}

		[HttpPost("return-books")]
		public async Task<string[]> ReturnBooks([FromBody] List<TransactionDtos> returnRequests)
		{
			var results = new ConcurrentBag<string>();

			// Run each borrowing operation in parallel
			Parallel.ForEach(returnRequests, request =>
			{
				try
				{
					var result = _bookTransService.ReturnBookAsync(request.UserId, request.BookId, (DateTime)request.ReturnDate).Result;
					results.Add(result);
				}
				catch (Exception ex)
				{
					results.Add($"Failed to return book with ID {request.BookId} for user {request.UserId}: {ex.Message}");
				}
			});
			return results.ToArray();
		}

		[HttpGet("search-book")]
		public async Task<IActionResult> SearchBooks([FromQuery] string keyword)
		{
			if (string.IsNullOrWhiteSpace(keyword))
			{
				return BadRequest("Search keyword cannot be null or empty.");
			}

			try
			{
				var books = await _bookService.SearchBooksAsync(keyword);
				if (!books.Any())
				{
					return NotFound("No books found matching the keyword.");
				}

				return Ok(books);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to search for books: " + ex.Message);
			}
		}

	}
}
