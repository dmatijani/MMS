using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task Create(User entity)
		{
			await _context.Users.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(User entity)
		{
			_context.Users.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<User>> Get()
		{
			return await _context.Users.Include(u => u.Role).Include(u => u.UserData).ToListAsync();
		}

		public async Task<User?> Get(int id)
		{
			return await _context.Users.Include(u => u.Role).Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task Update(User entity)
		{
			_context.Users.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
