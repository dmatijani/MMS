using MMS.Models;

namespace MMS.Services.Responses
{
	public record ApprovedRequestServiceResponse(bool Flag, User? user = null, string Message = null!);
}
