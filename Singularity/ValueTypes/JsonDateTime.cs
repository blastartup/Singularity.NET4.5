using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public struct JsonDateTime
	{
		[DebuggerStepThrough]
		public JsonDateTime(Int32 year, Int32 month, Int32 day)
			: this(new DateTime(year, month, day))
		{
		}

		[DebuggerStepThrough]
		public JsonDateTime(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second)
			: this(new DateTime(year, month, day, hour, minute, second))
		{
		}

		[DebuggerStepThrough]
		public JsonDateTime(DateTime dateTime)
		{
			this._dateTime = SafeJsonDateTime(dateTime);
		}

		[DebuggerStepThrough]
		public JsonDateTime(DateTime? dateTime)
		{
			this._dateTime = SafeJsonDateTime(dateTime);
		}

		[DebuggerStepThrough]
		public JsonDateTime(String jsonDateTime)
		{
			this._dateTime = GetDateTime(jsonDateTime);
		}

		[DebuggerStepThrough]
		public static DateTime SafeJsonDateTime(DateTime dateTime)
		{
			return (!dateTime.IsEmpty() ? dateTime : DateTimeExtension.MinJsonValue);
		}

		[DebuggerStepThrough]
		public static DateTime? SafeJsonDateTime(DateTime? dateTime)
		{
			return dateTime != null ? (DateTime?)SafeJsonDateTime(dateTime.Value) : null;
		}

		[DebuggerStepThrough]
		public static DateTime? GetDateTime(String jsonDateTime)
		{
			DateTime? result = null;
			if (jsonDateTime != null)
			{
				result = DateTimeExtension.MinJsonValue;
				if (!jsonDateTime.IsEmpty())
				{
					result = jsonDateTime.ToDateTime();
				}
			}
			return result;
		}

		[DebuggerStepThrough]
		public override String ToString()
		{
			String result = null;
			if (_dateTime != null && !_dateTime.Value.IsEmpty() && _dateTime != DateTimeExtension.MinJsonValue)
			{
				result = _dateTime.Value.ToString(UtcFormat);
			}
			return result;
		}

		[DebuggerStepThrough]
		public static implicit operator String(JsonDateTime source)
		{
			return source.ToString();
		}

		[DebuggerStepThrough]
		public static implicit operator JsonDateTime(String source)
		{
			return new JsonDateTime(source);
		}

		[DebuggerStepThrough]
		public static implicit operator DateTime?(JsonDateTime source)
		{
			return source._dateTime;
		}

		[DebuggerStepThrough]
		public static implicit operator DateTime(JsonDateTime source)
		{
			return source._dateTime.ValueOnNull(DateTimeExtension.MinJsonValue);
		}

		[DebuggerStepThrough]
		public static implicit operator JsonDateTime(DateTime? source)
		{
			return new JsonDateTime(source);
		}

		[DebuggerStepThrough]
		public static implicit operator JsonDateTime(DateTime source)
		{
			return new JsonDateTime(source);
		}

		private DateTime? _dateTime;
		private const String UtcFormat = "u";
	}
}
