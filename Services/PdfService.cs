using iText.IO.Font;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.JSInterop;
using MMS.Models;

namespace MMS.Services
{
	public class PdfService : IPdfService
	{
		private readonly IJSRuntime _js;
		private readonly IWebHostEnvironment _env;

		public PdfService(IJSRuntime js, IWebHostEnvironment env)
		{
			_js = js;
			_env = env;
		}

		public byte[] GetMembershipConfirmation(User user)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfWriter writer = new PdfWriter(memoryStream);
				PdfDocument pdf = new PdfDocument(writer);

				GenerateMembershipConfirmation(pdf, user);

				byte[] bytes = memoryStream.ToArray();
				return bytes;
			}
		}

		public byte[] GetMembershipDecision(User user, string password)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfWriter writer = new PdfWriter(memoryStream);
				PdfDocument pdf = new PdfDocument(writer);

				GenerateMembershipDecision(pdf, user, password);

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

		private void GenerateMembershipConfirmation(PdfDocument pdf, User user)
		{
			Document document = new Document(pdf);

			string fontPath = Path.Combine(_env.WebRootPath, "arial.ttf");
			var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

			string footerText = $"Generirano {DateTime.Now.ToString("dd.MM.yyyy. u HH:mm:ss")} od strane MMS sustava";
			pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(document, font, footerText));

			Paragraph title = new Paragraph("Potvrda o članstvu")
				.SetTextAlignment(TextAlignment.CENTER)
				.SetFontSize(20)
				.SetBold()
				.SetFont(font);
			document.Add(title);

			document.Add(new Paragraph("\n").SetFont(font));

			document.Add(new Paragraph($"Zahtjev za članstvom poslan je {user.MembershipRequestDate.ToString("dd.MM.yyyy. u HH:mm:ss")}.").SetFont(font));
			document.Add(new Paragraph($"Profil člana/ice {user.Name} {user.Surname} odobren je datuma {user.MembershipApprovalDate.ToString("dd.MM.yyyy. u HH:mm:ss")}.").SetFont(font));
			if (user.Payments.Any())
			{
				Payment firstPayment = user.Payments.OrderBy(p => p.Date).First();
				Payment lastPayment = user.Payments.OrderByDescending(p => p.Date).First();
				document.Add(new Paragraph($"Datum prve uplate je {firstPayment.Date.ToString("dd.MM.yyyy.")}").SetFont(font));
				document.Add(new Paragraph($"Datum zadnje uplate je {lastPayment.Date.ToString("dd.MM.yyyy.")} i članstvo vrijedi do {lastPayment.DateUntil.ToString("dd.MM.yyyy.")} (još {(lastPayment!.DateUntil - DateTime.Now.Date).Days} dana).").SetFont(font));
			}

			document.Add(new Paragraph("\n").SetFont(font));

			document.Add(new Paragraph($"Ime: {user.Name}").SetFont(font));
			document.Add(new Paragraph($"Prezime: {user.Surname}").SetFont(font));
			document.Add(new Paragraph($"Email: {user.Email}").SetFont(font));

			if (user.UserData.Any())
			{
				foreach (var contact in user.UserData)
				{
					document.Add(new Paragraph($"{contact.Name}: {contact.Value}").SetFont(font));
				}
			}

			document.Add(new Paragraph("\n").SetFont(font));

			Paragraph paymentsTitle = new Paragraph("Popis plaćanja")
				.SetTextAlignment(TextAlignment.LEFT)
				.SetFontSize(16)
				.SetBold()
				.SetFont(font);
			document.Add(paymentsTitle);

			if (user.Payments.Any())
			{
				Table paymentsTable = new Table(new float[] { 1, 1 });
				paymentsTable.SetWidth(UnitValue.CreatePercentValue(100));
				paymentsTable.AddHeaderCell("Datum plaćanja").SetFont(font);
				paymentsTable.AddHeaderCell("Vrijedi do").SetFont(font);

				foreach (var payment in user.Payments.OrderByDescending(p => p.Date))
				{
					paymentsTable.AddCell(new Cell().Add(new Paragraph(payment.Date.ToString("dd.MM.yyyy."))).SetFont(font));
					paymentsTable.AddCell(new Cell().Add(new Paragraph(payment.DateUntil.ToString("dd.MM.yyyy."))).SetFont(font));
				}
				document.Add(paymentsTable).SetFont(font);
			}
			else
			{
				document.Add(new Paragraph("Nema zabilježenih plaćanja.").SetFont(font));
			}

			document.Close();
		}

		private void GenerateMembershipDecision(PdfDocument pdf, User user, string password)
		{
			Document document = new Document(pdf);

			string fontPath = Path.Combine(_env.WebRootPath, "arial.ttf");
			var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

			string footerText = $"Generirano {DateTime.Now.ToString("dd.MM.yyyy. u HH:mm:ss")} od strane MMS sustava";
			pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(document, font, footerText));

			Paragraph title = new Paragraph("Rješenje o članstvu")
				.SetTextAlignment(TextAlignment.CENTER)
				.SetFontSize(20)
				.SetBold()
				.SetFont(font);
			document.Add(title);

			document.Add(new Paragraph("\n").SetFont(font));

			document.Add(new Paragraph($"Zahtjev za članstvom poslan je {user.MembershipRequestDate.ToString("dd.MM.yyyy. u HH:mm:ss")}.").SetFont(font));
			document.Add(new Paragraph($"Profil člana/ice {user.Name} {user.Surname} odobren je datuma {user.MembershipApprovalDate.ToString("dd.MM.yyyy. u HH:mm:ss")}.").SetFont(font));

			document.Add(new Paragraph("\n").SetFont(font));

			document.Add(new Paragraph($"Ime: {user.Name}").SetFont(font));
			document.Add(new Paragraph($"Prezime: {user.Surname}").SetFont(font));
			document.Add(new Paragraph($"Email: {user.Email}").SetFont(font));

			if (user.UserData.Any())
			{
				foreach (var contact in user.UserData)
				{
					document.Add(new Paragraph($"{contact.Name}: {contact.Value}").SetFont(font));
				}
			}

			document.Add(new Paragraph("\n").SetFont(font));

			Paragraph credentialsTitle = new Paragraph("Pristupni podaci")
				.SetTextAlignment(TextAlignment.LEFT)
				.SetFontSize(16)
				.SetBold()
				.SetFont(font);
			document.Add(credentialsTitle);

			document.Add(new Paragraph("Za prijavu u sustav, koristite ove pristupne podatke.").SetFont(font));
			document.Add(new Paragraph($"Email: {user.Email}").SetFont(font));
			document.Add(new Paragraph($"Lozinka: {password}").SetFont(font));

			document.Add(new Paragraph("\n").SetFont(font));

			Paragraph paymentInfoTitle = new Paragraph("Podaci za plaćanje")
				.SetTextAlignment(TextAlignment.LEFT)
				.SetFontSize(16)
				.SetBold()
				.SetFont(font);
			document.Add(paymentInfoTitle);

			document.Add(new Paragraph("Vaš je račun odobren, no potrebno je prvo platiti članarinu. Članarina se plaća za godinu i članstvo vrijedi godinu dana od dana plaćanja. Nakon što napravite uplatu, prijavite se na Vaš profil te Vam je tamo omogućeno priložiti potvrdu o plaćanju.").SetFont(font));
			document.Add(new Paragraph("IBAN: HR1234567890123456789").SetFont(font));
			document.Add(new Paragraph("Iznos: 50€").SetFont(font));
			document.Add(new Paragraph($"Opis plaćanja: Uplata članarine za godinu {DateTime.Now.Year}").SetFont(font));
			document.Add(new Paragraph("Napišite godinu za koju plaćate kada budete plaćali članarinu idućih godina.").SetFont(font));

			document.Close();
		}
	}
}
