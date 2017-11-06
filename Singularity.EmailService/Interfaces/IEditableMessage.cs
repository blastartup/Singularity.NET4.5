using System;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	public interface IEditableMessage : IStaticMessage
	{
		new IMessageFrom From { get; set; }
		new String Subject { get; set; }
		new String Body { get; set; }
	}
}
