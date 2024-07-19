using MMS.Models;

namespace MMS.Data.Repositories
{
	public interface IUserDataRepository
	{
		public Task<List<UserData>> Get();
		public Task<UserData?> Get(int id);
		public Task<List<UserData>> GetByUser(User user);
		public Task Create(UserData entity);
		public Task Update(UserData entity);
		public Task Delete(UserData entity);
	}
}
