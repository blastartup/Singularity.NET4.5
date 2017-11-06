using System;
using System.Net.Mail;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	public interface IEmailMessage : IEditableMessage
	{
		EmailAddressCollection To { get; }
		EmailAddressCollection Bcc { get; }
		Boolean IsBodyHtml { get; set; }
		MailMessage ToMailMessage();
	}
}
