using Microsoft.Extensions.Options;
using MMS.Services.Mail;
using System.Net.Mail;

namespace MMS.Services
{
	public class MailService
	{
		private readonly SmtpSettings _smtpSettings;

		public MailService(IOptions<SmtpSettings> smtpSettings)
		{
			_smtpSettings = smtpSettings.Value;
		}

		public MailMessage MakeMailMessage(string to, string subject, string body, bool isHtml = false)
		{
			MailMessage mail = new MailMessage(_smtpSettings.Username, to);
			mail.Subject = subject;
			mail.Body = body;
			mail.IsBodyHtml = isHtml;

			return mail;
		}

		public bool SendMail(MailMessage mail)
		{
			try
			{
				using (SmtpClient smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
				{
					smtp.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
					smtp.EnableSsl = _smtpSettings.EnableSsl;
					smtp.Send(mail);
				}

				return true;
			} catch (Exception ex)
			{
				return false;
			}
		}
	}
}
