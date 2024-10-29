using LMS.DTOs;
using LMS.Models;

namespace LMS.Interfaces.ServicesInterface
{
	public interface ITransactionService
	{
		Task<List<BookTransaction>> CheckOverdueBooksAsync(DateTime currentDate);
		Task<string> BorrowBookAsync(int userId, int bookId, DateTime borrowDt, DateTime dueDt);
		Task<string> ReturnBookAsync(int userId, int bookId, DateTime returnDt);

	}
}
