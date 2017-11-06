using System;
using System.Diagnostics;
using System.Net;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	/// <summary>
	/// Resonse object for Data Access (Layer) actions.
	/// </summary>
	[DebuggerStepThrough]
	public class SendEmailResponse : ISendEmailResponse
	{
		/// <summary>
		/// Instantiate a default SendEmailResponse with an unknown status code and an empty description.
		/// </summary>
		public SendEmailResponse() : this(SendEmailCode.Unknown, String.Empty) { }

		/// <summary>
		/// Instantiate SendEmailResponse with the given status code and an empty description.
		/// </summary>
		public SendEmailResponse(SendEmailCode statusCode)
			: this(statusCode, String.Empty)
		{
		}

		/// <summary>
		/// Instantiate SendEmailResponse with an unknown status code and the given description.
		/// </summary>
		public SendEmailResponse(String description)
			: this(SendEmailCode.Unknown, description)
		{
		}

		/// <summary>
		/// Instantiate SendEmailResponse with the given status code and the given description.
		/// </summary>
		public SendEmailResponse(SendEmailCode statusCode, String description)
		{
			StatusCode = statusCode;
			Description = description;
		}

		/// <summary>
		/// Current status for this repsonse.
		/// </summary>
		public SendEmailCode StatusCode { get; set; }

		public String Description { get; set; }
	}

	public class SendEmailResponse<T> : SendEmailResponse 
	{
		/// <summary>
		/// Instantiate a default SendEmailResponse with an unknown status code, an empty description and a default value of type T.
		/// </summary>
		public SendEmailResponse() : this(SendEmailCode.Unknown, String.Empty, default(T)) { }

		/// <summary>
		/// Instantiate SendEmailResponse with an unknown status code, an empty description and given generic value of type T.
		/// </summary>
		public SendEmailResponse(T value)
			: this(SendEmailCode.Unknown, String.Empty, value)
		{
		}

		/// <summary>
		/// Instantiate SendEmailResponse with the given status code, an empty description and given generic value of type T.
		/// </summary>
		public SendEmailResponse(SendEmailCode statusCode, T value)
			: this(statusCode, String.Empty, value)
		{
		}

		/// <summary>
		/// Instantiate SendEmailResponse with the given status code, the given description and given generic value of type T.
		/// </summary>
		public SendEmailResponse(SendEmailCode statusCode, String description, T value)
			: base(statusCode, description)
		{
			Value = value;
		}

		/// <summary>
		/// Generic Value of type T
		/// </summary>
		public T Value { get; set; }
	}
}
