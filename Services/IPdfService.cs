using iText.Kernel.Pdf;
using MMS.Models;

namespace MMS.Services
{
	public interface IPdfService
	{
		public byte[] GetMembershipConfirmation(User user);

		public byte[] GetMembershipDecision(User user, string password);

		public Task DownloadPdf(byte[] pdfBytes, string filename);
	}
}
