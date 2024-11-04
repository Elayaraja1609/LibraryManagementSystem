using LMS.DTOs;
using LMS.Models;

namespace LMS.Interfaces.ServicesInterface
{
	public interface ITransactionService
	{
		Task<List<BookTransaction>> CheckOverdueBooksAsync(DateTime currentDate);
		Task<List<string>> BorrowBooksAsync(List<TransactionDtos> transactions);
		Task<List<string>> ReturnBooksAsync(List<TransactionDtos> transactions);

	}
}
