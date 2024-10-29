using LMS.Interfaces;
using LMS.Interfaces.ServicesInterface;
using LMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace LMS.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly string _connectionString;
		private readonly IUnitOfWork _unitOfWork;
		public TransactionService(IUnitOfWork unitOfWork, IConfiguration configuration)
		{
			_unitOfWork = unitOfWork;
			_connectionString = configuration.GetConnectionString("DefaultConnection");
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
