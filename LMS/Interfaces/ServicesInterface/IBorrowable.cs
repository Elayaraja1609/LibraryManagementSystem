using LMS.Models;

namespace LMS.Interfaces.ServicesInterface
{
	public interface IBorrowable
	{
		void Borrow(User user);
		void Return(User user);
	}
}
