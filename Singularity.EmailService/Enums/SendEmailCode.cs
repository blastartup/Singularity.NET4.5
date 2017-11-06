using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	/// <summary>
	/// Data access status codes for data access layer actions
	/// </summary>
	public enum SendEmailCode
	{
		/// <summary>
		/// So far nothing has happened or no idea what just happened.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Email was sent.
		/// </summary>
		Sent,
		/// <summary>
		/// Email was not sent.
		/// </summary>
		NotSent,
		/// <summary>
		/// Sending exception triggered.
		/// </summary>
		SmtpError,
	}
}