using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using LMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LMS.Controllers
{
	
	[Route("v1/book")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly IBookService _bookService; 
		public BooksController(IBookService bookService)
		{
			_bookService = bookService;
		}
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetBooks()
		{
			try
			{
				var books = await _bookService.GetAllBooksAsync();
				if (books == null || !books.Any())
				{
					throw new LMSException("No Record found");
				}

				return Ok(books);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to get the book: " + ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddBook([FromBody] BookDtos bookDto)
		{
			try
			{
				if (bookDto == null)
				{
					throw new LMSException("Failed to add the book.");
				}

				var bookId = await _bookService.AddBookAsync(bookDto);
				bookDto.Id = bookId;
				return CreatedAtAction(nameof(GetBooks), new { id = bookId }, bookDto);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the book: " + ex.Message);
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> updateBook(int id, [FromBody] BookDtos bookDto)
		{
			try
			{
				if (id <= 0 || bookDto == null)
				{
					throw new LMSException("Failed to update the book");
				}

				var result = await _bookService.UpdateBookAsync(id, bookDto);

				if (!result)
				{
					throw new LMSException($"Failed to update the book {id}.");
				}

				return CreatedAtAction(nameof(GetBooks), new { id = bookDto.Id }, bookDto);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the book: " + ex.Message);
			}
		}
		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> deleteBook(int id)
		{
			try
			{
				var success = await _bookService.DeleteBookAsync(id);
				if (!success)
				{
					throw new LMSException($"Failed to delete the book.");
				}

				return Ok();
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to delete the book: " + ex.Message);
			}
		}
		[HttpGet("search")]
		public async Task<IActionResult> SearchBooks([FromQuery] string keyword)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(keyword))
				{
					throw new LMSException("Failed to search the book");
				}

				var books = await _bookService.SearchBooksAsync(keyword);
				if (!books.Any())
				{
					throw new LMSException("Failed to search the book.");
				}

				return Ok(books);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to search the book: " + ex.Message);
			}
		}
		//[HttpPost("borrow")]
		//public async Task<IActionResult> BorrowBook([FromBody] BorrowRequestDtos request)
		//{
		//	await _semaphore.WaitAsync();
		//	try
		//	{
		//		if (request == null)
		//		{
		//			throw new LMSException("Failed to add the book.");
		//		}
		//		TransactionDtos transaction = new TransactionDtos();
		//		transaction.TransactionDate = DateOnly.FromDateTime(DateTime.Now);
		//		transaction.DueDate = DateOnly.FromDateTime(request.DueDate);
		//		transaction.BookId= request.BookId;
		//		transaction.UserId = request.UserId;
		//		var rel = await _transactionService.BorrowBookAsync(transaction);

		//		if (rel)
		//		{
		//			return CreatedAtAction(nameof(GetBooks), new { id = request.BookId }, request);
		//		}
		//		else
		//		{
		//			return BadRequest("Failed to update the borrow details");
		//		}
		//	}
		//	catch (LMSException ex)
		//	{
		//		return BadRequest(ex.Message);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, "Failed to update the borrowed book: " + ex.Message);
		//	}
		//	finally
		//	{
		//		_semaphore.Release();
		//	}
		//}
		//[HttpPost("return")]
		//public async Task<IActionResult> ReturnBook([FromBody] ReturnRequestDtos request)
		//{
		//	await _semaphore.WaitAsync();
		//	try
		//	{
		//		if (request == null)
		//		{
		//			throw new LMSException("Failed to add the book.");
		//		}

		//		var trans = await _transactionService.GetAllTransAsync();
		//		var returnreq = trans.Where(x => x.Id == request.TransactionId).FirstOrDefault();
		//		returnreq.ReturnDate = DateOnly.FromDateTime(request.ReturnDate);
		//		var rel = await _transactionService.ReturnBookAsync(returnreq);

		//		if (rel)
		//		{
		//			return CreatedAtAction(nameof(GetBooks), new { id = request.BookId }, request);
		//		}
		//		else
		//		{
		//			return BadRequest("Failed to update the borrow details");
		//		}
		//	}
		//	catch (LMSException ex)
		//	{
		//		return BadRequest(ex.Message);
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, "Failed to update the borrowed book: " + ex.Message);
		//	}
		//	finally
		//	{
		//		_semaphore.Release();
		//	}
		//}
	}
}
