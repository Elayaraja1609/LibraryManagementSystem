using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
	[Route("v1/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserServices _userService;
		public UserController(IUserServices userService)
		{
			_userService = userService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var result = await _userService.GetAllUsersAsync();
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
				return StatusCode(500, "Failed to get the User details: " + ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddResr([FromBody] UserDtos updatedUser)
		{
			try
			{
				if (updatedUser == null)
				{
					throw new LMSException("Failed to add the User details.");
				}

				var id = await _userService.AddUserAsync(updatedUser);
				updatedUser.Id = id;
				return CreatedAtAction(nameof(GetAll), new { id = id }, updatedUser);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the User details: " + ex.Message);
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UserDtos updatedUser)
		{
			try
			{
				if (id <= 0 || updatedUser == null)
				{
					throw new LMSException("Failed to update the User details");
				}

				var result = await _userService.UpdateUserAsync(id, updatedUser);

				if (!result)
				{
					throw new LMSException($"Failed to update the User details {id}.");
				}

				return CreatedAtAction(nameof(GetAll), new { id = updatedUser.Id }, updatedUser);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the User details: " + ex.Message);
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var success = await _userService.DeleteUserAsync(id);
				if (!success)
				{
					throw new LMSException($"Failed to delete the User details.");
				}

				return Ok();
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to delete the User details: " + ex.Message);
			}
		}
	}
}
