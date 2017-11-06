using System;
using System.Net.Mail;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	/// <summary>
	/// EmailMessage is just an easier implementation of the .NET MailMessage.  But unlike the Email (Communications) entity,
	/// this class can look after multiple recipients.  The entity version only deals with a singular recipient.
	/// </summary>
	public class EmailMessage : IEmailMessage
	{
		public EmailMessage()
		{
			_isBodyHtml = true;
			_dateScheduled = DateTime.UtcNow;
			_notification = new Notification();
		}

		public MailMessage ToMailMessage()
		{
			MailMessage mailMessage = null;
			if (IsValid)
			{
				mailMessage = new MailMessage
				{
					From = (EmailAddress)From,
					Subject = Subject,
					Body = Body,
					IsBodyHtml = IsBodyHtml,
				};

				if (!To.IsEmpty())
				{
					To.ForEach(e => mailMessage.To.Add(e));
				}

				if (!Bcc.IsEmpty())
				{
					Bcc.ForEach(e => mailMessage.Bcc.Add(e));
				}
			}
			return mailMessage;
		}

		public IMessageFrom From { get; set; }

		public EmailAddressCollection To 
		{
			get { return _to ?? (_to = new EmailAddressCollection()); }
		}

		private EmailAddressCollection _to;

		public EmailAddressCollection Bcc
		{
			get { return _bcc ?? (_bcc = new EmailAddressCollection()); }
		}

		private EmailAddressCollection _bcc;

		public String Subject { get; set; }
		public String Body { get; set; }

		public DateTime DateScheduled
		{
			get { return _dateScheduled; }
			set { _dateScheduled = value; }
		}

		private DateTime _dateScheduled;

		public Boolean IsBodyHtml
		{
			get { return _isBodyHtml; }
			set { _isBodyHtml = value; }
		}

		private Boolean _isBodyHtml;

		public virtual Boolean IsValid
		{
			get { return From != null && (To != null || Bcc != null) && Subject != null && Body != null; }
		}

		public INotification Notification 
		{
			get { return _notification; }
			set { _notification = value; }
		}

		private INotification _notification;
	}
}
