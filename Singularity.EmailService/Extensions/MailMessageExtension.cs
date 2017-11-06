using System.Diagnostics;
using System.Net.Mail;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	[DebuggerStepThrough]
	public static class MailMessageExtension
	{
		/// <summary>
		/// Clone a MailMessage
		/// </summary>
		/// <param name="templateMailMessage">MailMessage to clone.</param>
		/// <returns>A cloned MailMessage.</returns>
		public static MailMessage Clone(this MailMessage templateMailMessage)
		{
			MailMessage result = new MailMessage();
			if (templateMailMessage == null)
			{
				return result;
			}

			result.From = templateMailMessage.From;
			result.Subject = templateMailMessage.Subject;
			result.Body = templateMailMessage.Body;
			result.IsBodyHtml = templateMailMessage.IsBodyHtml;
			result.DeliveryNotificationOptions = templateMailMessage.DeliveryNotificationOptions;
			result.Priority = templateMailMessage.Priority;
			result.Sender = templateMailMessage.Sender;
			result.SubjectEncoding = templateMailMessage.SubjectEncoding;
			result.BodyEncoding = templateMailMessage.BodyEncoding;

			if (!templateMailMessage.To.IsEmpty())
			{
				result.To.AddRange(templateMailMessage.To);
			}

			if (!templateMailMessage.CC.IsEmpty())
			{
				result.CC.AddRange(templateMailMessage.CC);
			}

			if (!templateMailMessage.Bcc.IsEmpty())
			{
				result.Bcc.AddRange(templateMailMessage.Bcc);
			}

			if (!templateMailMessage.ReplyToList.IsEmpty())
			{
				result.ReplyToList.AddRange(templateMailMessage.ReplyToList);
			}

			if (!templateMailMessage.Attachments.IsEmpty())
			{
				foreach (Attachment attachment in templateMailMessage.Attachments)
				{
					result.Attachments.Add(attachment);
				}
			}

			if (!templateMailMessage.AlternateViews.IsEmpty())
			{
				foreach (AlternateView alternateView in templateMailMessage.AlternateViews)
				{
					result.AlternateViews.Add(alternateView);
				}
			}

			return result;
		}

	}
}
