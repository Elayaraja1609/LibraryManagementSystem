using LMS.Data;
using LMS.DTOs;
using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using LMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly string _connectionString;
		private readonly IUnitOfWork _unitOfWork;
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
		public TransactionService(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}
		public async Task<IEnumerable<TransactionDtos>> GetAllTransAsync()
		{
			var trans = await _unitOfWork.BookTransactions.GetAllAsync();
			return trans.Select(a => new TransactionDtos
			{
				Id = a.Id,
				TransactionDate= a.TransactionDate,
				DueDate= a.DueDate,
				ReturnDate=	a.ReturnDate,
				BookId= a.BookId,
				BookName=a.Book != null? a.Book.BookTitle:null,
				UserId= a.UserId,
				UserName=a.User !=null? a.User.Username:null,
				status=a.status
			});
		}
		public async Task<int> AddTransAsync(TransactionDtos updatedTrans)
		{
			int id = 0;
			try
			{
				var trans = new BookTransaction
				{
					Id = updatedTrans.Id,
					TransactionDate = updatedTrans.TransactionDate,
					DueDate = updatedTrans.DueDate,
					ReturnDate = updatedTrans.ReturnDate,
					BookId = updatedTrans.BookId,
					UserId = updatedTrans.UserId,
					status = updatedTrans.status
				};
				_ = _unitOfWork.BookTransactions.AddAsync(trans);
				await _unitOfWork.CompleteAsync();
				id = trans.Id;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return id;
		}
		public async Task<bool> UpdateTransAsync(int id, TransactionDtos updatedTrans)
		{
			var trans = await _unitOfWork.BookTransactions.GetByIdAsync(id);
			if (trans == null)
			{
				return false;
			}

			trans.Id = updatedTrans.Id;
			trans.TransactionDate = updatedTrans.TransactionDate;
			trans.DueDate = updatedTrans.DueDate;
			trans.ReturnDate = updatedTrans.ReturnDate;
			trans.BookId = updatedTrans.BookId;
			trans.UserId = updatedTrans.UserId;
			trans.status = updatedTrans.status;


			await _unitOfWork.CompleteAsync();

			return true;
		}
		public async Task<bool> DeleteTransAsync(int transId)
		{
			var trans = await _unitOfWork.BookTransactions.GetByIdAsync(transId);
			if (trans == null) return false;

			_unitOfWork.BookTransactions.Remove(trans);
			await _unitOfWork.CompleteAsync();
			return true;
		}

		public async Task<bool> BorrowBookAsync(TransactionDtos trans)
		{
			await _semaphore.WaitAsync();
			try
			{
				var books = await _unitOfWork.Books.GetAllAsync();
				var book = books.Where(x => x.BookId == trans.BookId).FirstOrDefault();

				if (book == null || !book.IsAvailable)
				{
					return false;
				}

				book.BookQty--;
				if(book.BookQty<= 0)
				{
					book.IsAvailable = false;
				}
				await AddTransAsync(trans);
				await _unitOfWork.CompleteAsync();

				return true;
			}
			finally
			{
				_semaphore.Release();
			}
		}
		public async Task<bool> ReturnBookAsync(TransactionDtos trans)
		{
			await _semaphore.WaitAsync();
			try
			{
				var books = await _unitOfWork.Books.GetAllAsync();
				var book = books.Where(x => x.BookId == trans.BookId).FirstOrDefault();

				if (book == null || !book.IsAvailable)
				{
					return false;
				}

				book.BookQty++;
				if (book.BookQty >= 1)
				{
					book.IsAvailable = true;
				}
				
				await UpdateTransAsync(trans.Id, trans);
				await _unitOfWork.CompleteAsync();

				return true;
			}
			finally
			{
				_semaphore.Release();
			}
		}
		public async Task<bool> UpdateBookStatusAsync(int TransId)
		{
			var Trans = await _unitOfWork.BookTransactions.GetByIdAsync(TransId);
			if (Trans != null)
			{
				Trans.status = false;
				await _unitOfWork.CompleteAsync();
				return true;
			}
			return false;
		}
		public async Task<List<BookTransaction>> CheckOverdueBooksAsync(DateTime currentDate)
		{
			var trans = await _unitOfWork.BookTransactions.GetAllAsync();
			return trans.Where(r => r.DueDate <= currentDate && r.status).ToList();
		}
		public async Task<string> BorrowBookAsync(int userId, int bookId,DateTime borrowDt,DateTime dueDt)
		{
			string result = null;
			using (SqlConnection conn = new SqlConnection(_connectionString))
			using (SqlCommand cmd = new SqlCommand("spBorrowBook", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				List<SqlParameter> param = new List<SqlParameter>
				{
					new SqlParameter("@UserId", userId),
					new SqlParameter("@BookId", bookId),
					new SqlParameter("@BorrowDate", borrowDt),
					new SqlParameter("@DueDate", dueDt)
				};
				foreach (var parameter in param)
				{
					cmd.Parameters.Add(parameter);
				}
				await conn.OpenAsync();
				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
				{
					DataTable dataTable = new DataTable();
					await Task.Run(() => adapter.Fill(dataTable)); // Adapter fill is not inherently async
					if(dataTable.Rows.Count > 0)
					{
						result= dataTable.Rows[0]["Message"].ToString();
					}
				}
			}
			
			return result;
		}

		public async Task<string> ReturnBookAsync(int userId, int bookId, DateTime returnDt)
		{
			string result = null;
			using (SqlConnection conn = new SqlConnection(_connectionString))
			using (SqlCommand cmd = new SqlCommand("spReturnBook", conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				List<SqlParameter> param = new List<SqlParameter>
				{
					new SqlParameter("@UserId", userId),
					new SqlParameter("@BookId", bookId),
					new SqlParameter("@ReturnDate", returnDt)
				};
				foreach (var parameter in param)
				{
					cmd.Parameters.Add(parameter);
				}
				await conn.OpenAsync();
				using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
				{
					DataTable dataTable = new DataTable();
					await Task.Run(() => adapter.Fill(dataTable)); // Adapter fill is not inherently async
					if (dataTable.Rows.Count > 0)
					{
						result = dataTable.Rows[0]["Message"].ToString();
					}
				}
			}
			return result;
		}
	}
}
