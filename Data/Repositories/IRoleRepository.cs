using MMS.Models;

namespace MMS.Data.Repositories
{
	public interface IRoleRepository
	{
		public Task<List<Role>> Get();
		public Task<Role?> Get(int id);
		public Task Create(Role entity);
		public Task Update(Role entity);
		public Task Delete(Role entity);
	}
}
