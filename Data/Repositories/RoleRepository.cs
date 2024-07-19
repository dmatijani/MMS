using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Data.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		private readonly AppDbContext _context;

		public RoleRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task Create(Role entity)
		{
			await _context.Roles.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(Role entity)
		{
			_context.Roles.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Role>> Get()
		{
			return await _context.Roles.ToListAsync();
		}

		public async Task<Role?> Get(int id)
		{
			return await _context.Roles.Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task Update(Role entity)
		{
			_context.Roles.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
