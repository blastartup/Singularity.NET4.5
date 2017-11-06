using System.Diagnostics;
using System.Net;

namespace Singularity.Web
{
	[DebuggerStepThrough]
	public static class EnumExtension
	{
		public static EHttpStatusShortCode ShortCode(this HttpStatusCode value)
		{
			var numericValue = (int)value;
			if (numericValue > 500)
			{
				return EHttpStatusShortCode.Fail;
			}
			if (numericValue > 400)
			{
				return EHttpStatusShortCode.Error;
			}
			if (numericValue > 300)
			{
				return EHttpStatusShortCode.Warning;
			}
			if (numericValue > 200)
			{
				return EHttpStatusShortCode.OK;
			}
			return EHttpStatusShortCode.Ignore;
		}

		public static bool Success(this HttpStatusCode value)
		{
			return value.ShortCode() == EHttpStatusShortCode.OK;
		}
	}
}
