using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Services
{
	public class AuthService: IAuthService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;
		public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
		{
			_configuration = configuration;
			_unitOfWork = unitOfWork;
		}
		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			var users = await _unitOfWork.Users.GetAllAsync();
			return users.Select(a => new User
			{
				Id = a.Id,
				Firstname = a.Firstname,
				Lastname = a.Lastname,
				Username = a.Username,
				Password = a.Password,
				Email = a.Email,
				Contact = a.Contact,
				Address = a.Address,
				RoleId = a.RoleId,
				Active = a.Active
			});
		}
		public async Task<int?> LoginAsync(LoginDto dto)
		{
			var rel = await _unitOfWork.Users.GetAllAsync();
			var logedUser = rel.Where(u => u.Username == dto.Username && u.Password == dto.Password).FirstOrDefault();

			if (logedUser == null || !logedUser.Active)
			{
				return null;
			}
			var userlogin = new UserLogin
			{
				User= logedUser,
				UserLoginDate = DateTime.Now
			};

			await _unitOfWork.Users.AddUserLoginAsync(userlogin);
			await _unitOfWork.CompleteAsync();


			return logedUser.Id;
		}
		public async Task<string?> GenerateJwtToken(int userId, int? roleId)
		{
			var roles = await _unitOfWork.Roles.GetAllAsync();
			var matchedRole = roles.Where(x => x.Id == roleId).Select(x => x.RoleName).ToString();

			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims1 = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
				new("userid",userId.ToString())
			};
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject= new ClaimsIdentity(claims1),
				Expires= DateTime.Now.AddMinutes(double.Parse(jwtSettings["TokenExpiryInMinutes"])),
				Issuer= jwtSettings["Issuer"],
				Audience = jwtSettings["Audience"],
				SigningCredentials= credentials
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var jwt = tokenHandler.WriteToken(token);

			return jwt;
		}
	}
}
