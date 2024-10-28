using LMS.DTOs;
using LMS.Models;

namespace LMS.Interfaces.ServicesInterface
{
	public interface ITransactionService
	{
		Task<IEnumerable<TransactionDtos>> GetAllTransAsync();
		Task<int> AddTransAsync(TransactionDtos updatedTrans);
		Task<bool> UpdateTransAsync(int id, TransactionDtos updatedTrans);
		Task<bool> DeleteTransAsync(int transId);
		Task<bool> BorrowBookAsync(TransactionDtos trans);
		Task<bool> ReturnBookAsync(TransactionDtos trans);
		Task<bool> UpdateBookStatusAsync(int TransId);
		Task<List<BookTransaction>> CheckOverdueBooksAsync(DateTime currentDate);
		Task<string> BorrowBookAsync(int userId, int bookId, DateTime borrowDt, DateTime dueDt);
		Task<string> ReturnBookAsync(int userId, int bookId, DateTime returnDt);

	}
}
