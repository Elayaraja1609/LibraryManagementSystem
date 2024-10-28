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
	public class AuthorsController : ControllerBase
	{
		private readonly IAuthorService _authorService;

		public AuthorsController(IAuthorService authorService)
		{
			_authorService = authorService;
		}
		[HttpGet]
		public async Task<IActionResult> GetAuthors()
		{
			try
			{
				var result = await _authorService.GetAllAuthorAsync();
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
				return StatusCode(500, "Failed to get the authors: " + ex.Message);
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddAuthor([FromBody] AuthorDto newAuthor)
		{
			try
			{
				if (newAuthor == null)
				{
					throw new LMSException("Failed to add the author.");
				}

				var authorid = await _authorService.AddAuthorAsync(newAuthor);
				newAuthor.Id = authorid;
				return CreatedAtAction(nameof(GetAuthors), new { id = authorid }, newAuthor);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to add the author: " + ex.Message);
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDto updatedAuthor)
		{
			try
			{
				if (id <= 0 || updatedAuthor == null)
				{
					throw new LMSException("Failed to update the author");
				}

				var result = await _authorService.UpdateAuthorAsync(id, updatedAuthor);

				if (!result)
				{
					throw new LMSException($"Failed to update the author {id}.");
				}

				return CreatedAtAction(nameof(GetAuthors), new { id = updatedAuthor.Id }, updatedAuthor);
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to update the author: " + ex.Message);
			}
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAuthor(int id)
		{
			try
			{
				var success = await _authorService.DeleteAuthorAsync(id);
				if (!success)
				{
					throw new LMSException($"Failed to delete the author.");
				}

				return Ok();
			}
			catch (LMSException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Failed to delete the author: " + ex.Message);
			}
		}

	}
}
