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
		[HttpPost("borrow-book")]
		public async Task<IActionResult> BorrowBook([FromBody] TransactionDtos newTrans)
		{
			if (newTrans == null)
			{
				return BadRequest("Transaction details cannot be null.");
			}

			try
			{
				var id = await _bookTransService.BorrowBookAsync(newTrans.UserId, newTrans.BookId, newTrans.TransactionDate, newTrans.DueDate);
				if (id == "Book borrowed successfully.")
				{
					return Ok("Book borrowed successfully.");
				}

				return StatusCode(StatusCodes.Status500InternalServerError, id);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to borrow the book: " + ex.Message);
			}
		}
		[HttpPost("return-book")]
		public async Task<IActionResult> ReturnBook([FromBody] TransactionDtos newTrans)
		{
			if (newTrans == null)
			{
				return BadRequest("Transaction details cannot be null.");
			}

			try
			{
				var id = await _bookTransService.ReturnBookAsync(newTrans.UserId, newTrans.BookId, (DateTime)newTrans.ReturnDate);
				if (id == "Book returned successfully.")
				{
					return Ok("Book returned successfully.");
				}

				return StatusCode(StatusCodes.Status500InternalServerError, id);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return the book: " + ex.Message);
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
