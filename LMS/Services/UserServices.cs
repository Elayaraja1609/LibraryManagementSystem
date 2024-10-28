using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using System.Net;
using System;

namespace LMS.Services
{
	public class UserServices: IUserServices
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<UserDtos>> GetAllUsersAsync()
		{
			var users = await _unitOfWork.Users.GetAllAsync();
			return users.Select(a => new UserDtos
			{
				Id=a.Id,
				Firstname =a.Firstname,
				Lastname =a.Lastname,
				Username =a.Username,
				Password =a.Password,
				Email =a.Email,
				Contact =a.Contact,
				Address =a.Address,
				RoleId = a.RoleId,
				Active=a.Active
			});
		}
		public async Task<int> AddUserAsync(UserDtos updatedUser)
		{
			var user = new User
			{
				Id = updatedUser.Id,
				Firstname = updatedUser.Firstname,
				Lastname = updatedUser.Lastname,
				Username = updatedUser.Username,
				Password = updatedUser.Password,
				Email = updatedUser.Email,
				Contact = updatedUser.Contact,
				Address = updatedUser.Address,
				RoleId = updatedUser.RoleId,
				Active = updatedUser.Active
			};
			_ = _unitOfWork.Users.AddAsync(user);
			await _unitOfWork.CompleteAsync();
			return user.Id;
		}
		public async Task<bool> UpdateUserAsync(int id, UserDtos updatedUser)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);
			if (user == null)
			{
				return false;
			}

			user.Id = updatedUser.Id;
			user.Firstname = updatedUser.Firstname;
			user.Lastname = updatedUser.Lastname;
			user.Username = updatedUser.Username;
			user.Password = updatedUser.Password;
			user.Email = updatedUser.Email;
			user.Contact = updatedUser.Contact;
			user.Address = updatedUser.Address;
			user.RoleId = updatedUser.RoleId;
			user.Active = updatedUser.Active;


			await _unitOfWork.CompleteAsync();

			return true;
		}
		public async Task<bool> DeleteUserAsync(int userId)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(userId);
			if (user == null) return false;

			_unitOfWork.Users.Remove(user);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
