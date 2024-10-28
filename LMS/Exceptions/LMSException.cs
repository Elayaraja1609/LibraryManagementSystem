namespace LMS.Exceptions
{
	public class LMSException : Exception
	{
		public LMSException()
		{
		}

		public LMSException(string message) : base(message)
		{
		}

		public LMSException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
