using MMS.Models;

namespace MMS.Data.Repositories
{
	public interface IPaymentRepository
	{
		public Task<List<Payment>> Get();
		public Task<Payment?> Get(int id);
		public Task Create(Payment entity);
		public Task Update(Payment entity);
		public Task Delete(Payment entity);
	}
}
