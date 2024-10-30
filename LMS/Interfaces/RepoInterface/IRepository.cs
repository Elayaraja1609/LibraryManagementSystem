namespace LMS.Interfaces.RepoInterface
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task AddAsync(T dto);
		void Update(T dto);
		void Remove(T dto);
	}
}
