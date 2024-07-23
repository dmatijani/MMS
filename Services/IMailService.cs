using System.Net.Mail;

namespace MMS.Services
{
	public interface IMailService
	{
		public MailMessage MakeMailMessage(string to, string subject, string body, List<Attachment> attachments, bool isHtml);

		public Task<bool> SendMail(MailMessage mail);
	}
}
