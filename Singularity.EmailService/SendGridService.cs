using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Singularity.EmailService
{
	public class SendGridService
	{
		private static readonly String _username = "azure_77ec1b3446a7b3f453c4df97b8493aa0@azure.com";
		private static readonly String _password = "yarwp6xx";
		private MailMessage _mailMessage;

		/// <summary>
		/// Send an outgoing email using SMTP
		/// </summary>
		/// <param name="from">The single email address the message is being sent from.</param>
		/// <param name="recipients">The list of email addresses the message is being sent to.</param>
		/// <param name="subject">The subject of the message.</param>
		/// <param name="contents">The HTML message in one string.</param>
		public SendGridService(MailMessage mailMessage)
		{
			_mailMessage = mailMessage;
		}

		public async Task SendAsync()
		{
			using(SmtpClient client = new SmtpClient())
			{
				client.Port = 587;
				client.Host = "smtp.sendgrid.net";
				client.Timeout = 10000;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential(_username, _password);
				await client.SendMailAsync(_mailMessage);
			}
		}
	}
}
