using MMS.Models;
using MMS.Services.Responses;

namespace MMS.Services
{
	public interface IPaymentService
	{
		public Task<ServiceResponse> SavePayment(Payment payment);

		public Task<List<Payment>> GetPaymentsForUser(User user);
	}
}
