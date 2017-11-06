using System;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	public interface IStaticMessage : IStateValid 
	{
		IMessageFrom From { get; }
		String Subject { get; }
		String Body { get; }
	}

}
