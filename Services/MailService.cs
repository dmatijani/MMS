using Microsoft.Extensions.Options;
using MMS.Services.Mail;
using System.Net.Mail;

namespace MMS.Services
{
	public class MailService
	{
		private readonly SmtpSettings _smtpSettings;
		private string _from { get; set; }

		public MailService(IOptions<SmtpSettings> smtpSettings)
		{
			_smtpSettings = smtpSettings.Value;
			_from = _smtpSettings.Username;
		}

		public MailMessage MakeMailMessage(string to, string subject, string body, List<Attachment> attachments, bool isHtml = false)
		{
			MailMessage mail = new MailMessage(_from, to);
			mail.Subject = subject;
			mail.Body = body;
			mail.IsBodyHtml = isHtml;
			foreach (var attachment in attachments)
			{
				mail.Attachments.Add(attachment);
			}

			return mail;
		}

		public async Task<bool> SendMail(MailMessage mail)
		{
			try
			{
				using (SmtpClient smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
				{
					smtp.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
					smtp.EnableSsl = _smtpSettings.EnableSsl;
					await smtp.SendMailAsync(mail);
				}

				return true;
			} catch (Exception ex)
			{
				return false;
			}
		}
	}
}
