using MMS.Models.ViewModels;
using MMS.Models;
using MMS.Services.Responses;

namespace MMS.Services
{
	public interface IUserService
	{
		public Task<User?> GetUserByLoginInformation(LoginViewModel model);

		public Task<List<User>> GetNonApprovedUsers();

		public Task<User?> GetUserById(int userId, bool approved = true);

		public Task<ServiceResponse> RejectRequest(User user);

		public Task<ApprovedRequestServiceResponse> ApproveRequest(User user);

		public Task<ServiceResponse> SendNewMembershipRequest(MembershipRequestViewModel model);

		public Task<ServiceResponse> Update(User user);

		public Task AddAdminUser();
	}
}
