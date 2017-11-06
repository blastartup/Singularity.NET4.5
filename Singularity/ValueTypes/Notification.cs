using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public struct Notification : INotification
	{
		public Notification(String message)
		{
			this._message = message;
			_exception = null;
		}

		public Notification(Exception exception)
		{
			_message = exception.Message;
			this._exception = exception;
		}

		public String Message
		{
			get { return _message; }
			set { _message = value; }
		}

		private String _message;

		public Exception Exception
		{
			get { return _exception; }
			set { _exception = value; }
		}

		private Exception _exception;
	}
}
