using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Controllers
{
	[Route("v1/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ITransactionService _bookTransService;
		private readonly IBookService _bookService;
		private readonly IUserServices _userService;

		public UsersController(ITransactionService bookTransService, IBookService bookService, IUserServices userService)
		{
			_bookTransService = bookTransService;
			_bookService = bookService;
			_userService = userService;
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
		public async Task<IActionResult> BorrowBooks([FromBody] List<TransactionDtos> borrowRequests)
		{
			if (borrowRequests == null || !borrowRequests.Any())
			{
				return BadRequest("Transaction details cannot be null or empty.");
			}

			try
			{
				var tasks = borrowRequests.Select(async request =>
				{
					return await _bookTransService.BorrowBookAsync(request.UserId, request.BookId, request.TransactionDate, request.DueDate);
				});

				var results = await Task.WhenAll(tasks);

				return Ok(results);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to borrow books: " + ex.Message);
			}
		}

		[HttpPost("return-books")]
		public async Task<IActionResult> ReturnBooks([FromBody] List<TransactionDtos> returnRequests)
		{
			if (returnRequests == null || !returnRequests.Any())
			{
				return BadRequest("Transaction details cannot be null or empty.");
			}

			try
			{
				var tasks = returnRequests.Select(async request =>
				{
					return await _bookTransService.ReturnBookAsync(request.UserId, request.BookId, (DateTime)request.ReturnDate);
				});

				var results = await Task.WhenAll(tasks);

				return Ok(results);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return books: " + ex.Message);
			}
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
