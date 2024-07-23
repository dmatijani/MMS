using MMS.Models;

namespace MMS.Services.Responses
{
	public record ApprovedRequestServiceResponse(bool Flag, User? user, string password, string Message = null!);
}
