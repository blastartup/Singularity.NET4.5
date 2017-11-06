using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	[DebuggerStepThrough]
	public class StaticMessage : IStaticMessage
	{
		public StaticMessage(IMessageFrom from, String subject, String body)
		{
			_from = from;
			_subject = subject;
			_body = body;
		}

		public IMessageFrom From => _from;

		public String Subject => _subject;

		public String Body => _body;

		public Boolean IsValid
		{
			get { return !_subject.IsEmpty() && !_body.IsEmpty() && !_from.Address.IsEmpty(); }
		}

		private IMessageFrom _from;
		private String _subject, _body;
	}
}
