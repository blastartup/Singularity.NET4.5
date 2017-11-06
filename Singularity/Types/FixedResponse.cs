using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class FixedResponse : IFixedCondition
	{
		public FixedResponse()
		{
			Request = "All";
			Type = EFixedResponseTypes.Message;
		}

		public FixedResponse FormatX(params Object[] values)
		{
			Message = Message.FormatX(values);
			return this;
		}

		public FixedResponse With(DelimitedStringBuilder messages)
		{
			return FormatX(messages.ToDelimitedString(ValueLib.SemiColon.StringValue));
		}

		public Guid ApiFixedResponseId { get; set; }
		public String Code { get; set; }
		public String Message { get; set; }
		public String Request { get; set; }
		public EFixedResponseTypes Type { get; set; }
		public String AdditionalInfo { get; set; }

		public String CodeWithMessage => "{0};{1}".FormatX(Code, Message);
	}
}