using MMS.Data.Repositories;
using MMS.Models;
using MMS.Services.Responses;

namespace MMS.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IPaymentRepository _repo;

		public PaymentService(IPaymentRepository repository)
		{
			_repo = repository;
		}

		public async Task<ServiceResponse> SavePayment(Payment payment)
		{
			try
			{
				await _repo.Create(payment);
				return new ServiceResponse(true, "Uspješno");
			} catch (Exception ex)
			{
				return new ServiceResponse(false, "Problem s spajanjem na bazu.");
			}
		}

		public async Task<List<Payment>> GetPaymentsForUser(User user)
		{
			return (await _repo.Get()).Where(p => p.UserId == user.Id).ToList();
		}
	}
}
