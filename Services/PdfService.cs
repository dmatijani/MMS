using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.JSInterop;
using MMS.Models;

namespace MMS.Services
{
	public class PdfService
	{
		private IJSRuntime _js;

		public PdfService(IJSRuntime js)
		{
			_js = js;
		}

		public byte[] GenerateMembershipConfirmation(User user)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfWriter writer = new PdfWriter(memoryStream);
				PdfDocument pdf = new PdfDocument(writer);
				Document document = new Document(pdf);

				document.Add(new Paragraph("Hej, bok!"));
				document.Add(new Paragraph("Ovo je jednostavan PDF dokument!"));

				document.Close();

				byte[] bytes = memoryStream.ToArray();
				return bytes;
			}
		}

		public async Task DownloadPdf(byte[] pdfBytes, string filename = "document.pdf")
		{
			string documentBase64 = Convert.ToBase64String(pdfBytes);
			if (!filename.EndsWith(".pdf"))
			{
				filename += ".pdf";
			}
			await _js.InvokeVoidAsync("saveFile", filename, documentBase64);
		}
	}
}
