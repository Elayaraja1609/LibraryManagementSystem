using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using LMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
	[Route("v1/[controller]")]
	[ApiController]
	public class BookTransactionController : ControllerBase
	{
		private readonly ITransactionService _bookTransService;
		public BookTransactionController(ITransactionService bookTransService)
		{
			_bookTransService = bookTransService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var result = await _bookTransService.GetAllTransAsync();
				if (result == null || !result.Any())
				{
					throw new LMSException("No Record found");
				}

				return Ok(result);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to get the Transaction details: " + ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddTrans([FromBody] TransactionDtos newTrans)
		{
			try
			{
				if (newTrans == null)
				{
					throw new LMSException("Failed to add the Transaction details.");
				}

				var id = await _bookTransService.AddTransAsync(newTrans);
				newTrans.Id = id;
				return CreatedAtAction(nameof(GetAll), new { id = id }, newTrans);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the Transaction details: " + ex.Message);
			}
		}
		[HttpPut("{transId}")]
		public async Task<IActionResult> UpdateTrans(int transId, [FromBody] TransactionDtos updateTrans)
		{
			try
			{
				if (transId <= 0 || updateTrans == null)
				{
					throw new LMSException("Failed to update the Transaction details");
				}

				var result = await _bookTransService.UpdateTransAsync(transId, updateTrans);

				if (!result)
				{
					throw new LMSException($"Failed to update the Transaction details {transId}.");
				}

				return CreatedAtAction(nameof(GetAll), new { id = updateTrans.Id }, updateTrans);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the Transaction details: " + ex.Message);
			}
		}
		[HttpPut("updatebookstatus/{TransId}")]
		public async Task<IActionResult> UpdateBookStatus(int TransId)
		{
			try
			{
				var result = await _bookTransService.UpdateBookStatusAsync(TransId);
				if (!result)
				{
					return BadRequest("Failed to update book status.");
				}
				return Ok("Book status updated successfully.");
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the book status: " + ex.Message);
			}
		}
		[HttpGet("overduebooks/{currentDate}")]
		public async Task<IActionResult> CheckOverdueBooks(DateTime currentDate)
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
				return StatusCode(500, "Failed to check overdue: " + ex.Message);
			}
		}
		[HttpPost("borrow-book")]
		public async Task<IActionResult> BorrowBook([FromBody] TransactionDtos newTrans)
		{
			try
			{
				if (newTrans == null)
				{
					throw new LMSException("Failed to check the book details.");
				}

				var id = await _bookTransService.BorrowBookAsync(newTrans.UserId,newTrans.BookId,newTrans.TransactionDate,newTrans.DueDate);
				if (id == "Book borrowed successfully.") return Ok("Updated successfully.");
				else return StatusCode(500, id);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the Transaction details: " + ex.Message);
			}
		}
	}
}
