using MMS.Models.ViewModels;
using MMS.Services.Responses;

namespace MMS.Services
{
	public interface ILoginService
	{
		public Task<LoginServiceResponse> Login(HttpContext httpContext, LoginViewModel model);

		public Task<ServiceResponse> Logout(HttpContext httpContext);
	}
}
