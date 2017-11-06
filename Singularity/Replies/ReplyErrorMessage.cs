using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class ReplyErrorMessage : ReplySimple
	{
		public ReplyErrorMessage() : this(false)
		{
		}

		public ReplyErrorMessage(Boolean condition) : base(condition)
		{
		}

		public String ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				_errorMessage = value;
				Condition = value.IsEmpty();
			}
		}

		private String _errorMessage;
	}
}