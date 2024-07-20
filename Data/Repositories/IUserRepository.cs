using MMS.Models;

namespace MMS.Data.Repositories
{
	public interface IUserRepository
	{
		public Task<List<User>> Get();
		public Task<User?> Get(int id);
		public Task Create(User entity);
		public Task Update(User entity);
		public Task Delete(User entity);
	}
}
