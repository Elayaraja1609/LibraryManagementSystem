using LMS.DTOs;
using LMS.Exceptions;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.Controllers
{
	[Route("v1/admin")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IAuthorService _authorService;
		private readonly IBookService _bookService;
		private readonly IUserServices _userService;
		public AdminController(IAuthorService authorService, IBookService bookService, IUserServices userService)
		{
			_authorService = authorService;
			_bookService = bookService;
			_userService = userService;
		}
		#region Author

		[HttpGet("authors")]
		[SwaggerOperation(Summary = "Get all authors", Tags = new[] { "Author" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpPost("authors")]
		[SwaggerOperation(Summary = "Add a new author", Tags = new[] { "Author" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpPut("authors/{id}")]
		[SwaggerOperation(Summary = "Update an existing author", Tags = new[] { "Author" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpDelete("authors/{id}")]
		[SwaggerOperation(Summary = "Delete an author", Tags = new[] { "Author" })]
		[Authorize(Policy = "AdminPolicy")]
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
		#endregion

		#region Book
		[HttpGet("books")]
		[SwaggerOperation(Summary = "Get all books", Tags = new[] { "Book" })]
		[Authorize(Policy = "AdminOrUserPolicy")]
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

		[HttpPost("books")]
		[SwaggerOperation(Summary = "Add a new book", Tags = new[] { "Book" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpPut("books/{id}")]
		[SwaggerOperation(Summary = "Update an existing book", Tags = new[] { "Book" })]
		[Authorize(Policy = "AdminPolicy")]
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

		[HttpDelete("books/{id}")]
		[SwaggerOperation(Summary = "Delete a book", Tags = new[] { "Book" })]
		[Authorize(Policy = "AdminPolicy")]
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
		
		[HttpGet("search-book")]
		[SwaggerOperation(Summary = "Search books", Tags = new[] { "Book" })]
		[Authorize(Policy = "AdminOrUserPolicy")]
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
		#endregion

		#region User
		[HttpGet("users")]
		[SwaggerOperation(Summary = "Get all users", Tags = new[] { "User" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpPost("users")]
		[SwaggerOperation(Summary = "Add a new user", Tags = new[] { "User" })]
		[Authorize(Policy = "AdminPolicy")]
		public async Task<IActionResult> AddUser([FromBody] UserDtos updatedUser)
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
		[HttpPut("users/{id}")]
		[SwaggerOperation(Summary = "Update an existing user", Tags = new[] { "User" })]
		[Authorize(Policy = "AdminPolicy")]
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
		[HttpDelete("users/{id}")]
		[SwaggerOperation(Summary = "Delete a user", Tags = new[] { "User" })]
		[Authorize(Policy = "AdminPolicy")]
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
		#endregion
	}
}
