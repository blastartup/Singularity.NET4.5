using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Singularity
{
	/// <summary>
	/// Extension class for Exceptions
	/// </summary>
	[DebuggerStepThrough]
	public static class ExceptionExtension
	{
		/// <summary>
		/// Return exception details as a single string for logging purposes.
		/// </summary>
		/// <param name="exception">An exception whose details you want to log as a single string.</param>
		/// <returns>All the main properties of this exception and any inner exceptions as a single lined string.</returns>
		[DebuggerStepThrough]
		public static String ToLogString(this Exception exception)
		{
			var result = String.Empty;
			if (exception != null) 
			{
				var message = new DelimitedStringBuilder();
				message.Add("Exception:{0}", exception.GetType().Name);
				message.Add("Message:{0}", exception.Message);

				if (exception.Source != null)
				{
					message.Add("Source:" + exception.Source.Replace(ValueLib.EndOfLine.StringValue, ValueLib.Space.StringValue));
				}

				if (exception.Data.Count > 0)
				{
					message.Add("Addition Information:");
					message.Add(exception.Data.ToDescription());
				}

				if (!exception.HelpLink.IsEmpty())
				{
					message.Add("Help Link:" + exception.HelpLink);
				}

				if (exception.StackTrace != null)
				{
					message.Add("Stack Trace:" + exception.StackTrace);
				}

				if (exception.InnerException != null)
				{
					message.Add("Inner " + exception.InnerException.ToLogString());
				}
				result = message.ToDelimitedString(ValueLib.Comma.CharValue);
			}
			return result;
		}

		/// <summary>
		/// Return exception message including any inner exception messages only.
		/// </summary>
		/// <param name="exception">An exception whose message you want.</param>
		/// <returns>Only the exception message and any inner exception messages.</returns>
		[DebuggerStepThrough]
		public static String AsMessageOnly(this Exception exception)
		{
			var result = String.Empty;
			if (exception != null)
			{
				var message = new DelimitedStringBuilder();
				message.Add(exception.Message);

				if (exception.InnerException != null)
				{
					message.Add(exception.InnerException.AsMessageOnly());
				}
				result = message.ToDelimitedString("  ");
			}
			return result;
		}

		public static String LineNumber(this Exception exception)
		{
			var stackTrace = new StackTrace(exception, true);
			StackFrame[] stackFrames = stackTrace.GetFrames();
			if (stackFrames != null)
			{
				foreach (StackFrame stackFrame in stackFrames.Where(f => f.GetFileLineNumber() > 0))
				{
					return $"Line: {stackFrame.GetFileLineNumber()}";
				}
			}

			return "Line:Unknown";
		}

		private static IEnumerable<T> GetStackTraceWorkFlow<T>(Exception exception)
		{
			var traceSteps = new List<T>();
			Type attributeType = typeof(T);
			var stackTrace = new StackTrace(exception);
			if (stackTrace.FrameCount > 0)
			{
				for (var idx = stackTrace.FrameCount - 1; idx >= 0; idx--)
				{
					var attribute = stackTrace.GetFrame(idx).GetMethod().GetCustomAttributes(attributeType, false).FirstOrDefault();
					if (attribute != null)
					{
						traceSteps.Add((T)attribute);
					}
				}
			}
			return traceSteps;
		}
	}
}

