using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Data.Repositories
{
	public class UserDataRepository : IUserDataRepository
	{
		private readonly AppDbContext _context;

		public UserDataRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task Create(UserData entity)
		{
			await _context.UserData.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(UserData entity)
		{
			_context.UserData.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<UserData>> Get()
		{
			return await _context.UserData.ToListAsync();
		}

		public async Task<UserData?> Get(int id)
		{
			return await _context.UserData.Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<UserData>> GetByUser(User user)
		{
			return await _context.UserData.Where(x => x.UserId == user.Id).ToListAsync();
		}

		public async Task Update(UserData entity)
		{
			_context.UserData.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
