namespace MMS.Services.Responses
{
	public record LoginServiceResponse(bool Flag, string Role, string Message = null!);
}
