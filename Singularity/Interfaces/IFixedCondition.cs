using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface IFixedCondition : ICondition
	{
		Guid ApiFixedResponseId { get; }
		String Code { get; }
		String CodeWithMessage { get; }
		String Message { get; }
		String Request { get; }
		EFixedResponseTypes Type { get; }
		String AdditionalInfo { get; set; }
	}
}
