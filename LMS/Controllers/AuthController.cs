using LMS.DTOs;
using LMS.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
	[Route("v1/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var userId = await _authService.LoginAsync(loginDto);

			if (userId == null)
			{
				return Unauthorized(new { message = "Invalid username or password." });
			}


			var users = await _authService.GetAllUsersAsync();
			var logeduser = users.Where(x => x.Id == userId).Select(s => new { userid=s.Id,roleId=s.RoleId}).FirstOrDefault();
				

			if (logeduser == null)
			{
				return Unauthorized(new { message = "User not found." });
			}

			var token = _authService.GenerateJwtToken(logeduser.userid,logeduser.roleId);


			return Ok(new { Token = token.Result });
		}
	}
}
