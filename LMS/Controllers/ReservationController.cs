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
	public class ReservationController : ControllerBase
	{
		private readonly IReservationServices _resService;
		public ReservationController(IReservationServices resService)
		{
			_resService = resService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var result = await _resService.GetAllReservationAsync();
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
				return StatusCode(500, "Failed to get the Reservation details: " + ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddResr([FromBody] ReservationDtos newRes)
		{
			try
			{
				if (newRes == null)
				{
					throw new LMSException("Failed to add the Reservation details.");
				}

				var id = await _resService.AddReservationAsync(newRes);
				newRes.Id = id;
				return CreatedAtAction(nameof(GetAll), new { id = id }, newRes);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the Reservation details: " + ex.Message);
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateResr(int id, [FromBody] ReservationDtos updatedReserv)
		{
			try
			{
				if (id <= 0 || updatedReserv == null)
				{
					throw new LMSException("Failed to update the Reservation details");
				}

				var result = await _resService.UpdateReservationAsync(id, updatedReserv);

				if (!result)
				{
					throw new LMSException($"Failed to update the Reservation details {id}.");
				}

				return CreatedAtAction(nameof(GetAll), new { id = updatedReserv.Id }, updatedReserv);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the Reservation details: " + ex.Message);
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteReservation(int id)
		{
			try
			{
				var success = await _resService.DeleteReservationAsync(id);
				if (!success)
				{
					throw new LMSException($"Failed to delete the Reservation details.");
				}

				return Ok();
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to delete the Reservation details: " + ex.Message);
			}
		}
	}
}
