using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Data.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly AppDbContext _context;

		public PaymentRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task Create(Payment entity)
		{
			await _context.Payments.AddAsync(entity);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(Payment entity)
		{
			_context.Payments.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Payment>> Get()
		{
			return await _context.Payments.Include(p => p.User).ToListAsync();
		}

		public async Task<Payment?> Get(int id)
		{
			return await _context.Payments.Include(p => p.User).Where(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task Update(Payment entity)
		{
			_context.Payments.Update(entity);
			await _context.SaveChangesAsync();
		}
	}
}
