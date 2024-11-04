using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Policy = "AdminOrUserPolicy")]
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
		[Authorize(Policy = "AdminOrUserPolicy")]
		public async Task<IActionResult> BorrowBooks([FromBody] List<TransactionDtos> borrowRequests)
		{
			if (borrowRequests == null || !borrowRequests.Any())
				return BadRequest("Borrow request cannot be null or empty.");

			var result = await _bookTransService.BorrowBooksAsync(borrowRequests);

			if (result.All(r => r == "Book borrowed successfully."))
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpPost("return-books")]
		[Authorize(Policy = "AdminOrUserPolicy")]
		public async Task<IActionResult> ReturnBooks([FromBody] List<TransactionDtos> returnRequests)
		{
			if (returnRequests == null || !returnRequests.Any())
				return BadRequest("Return request cannot be null or empty.");

			var result = await _bookTransService.ReturnBooksAsync(returnRequests);

			if (result.All(r => r == "Book returned successfully."))
				return Ok(result);
			else
				return StatusCode(500, result);
		}

		[HttpGet("search-book")]
		[Authorize(Policy = "AdminOrUserPolicy")]
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
