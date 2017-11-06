using System;
using System.Web;

namespace Singularity.Web
{
	public static class QueryString
	{
		public static Guid GetIdAsGuid()
		{
			return HttpContext.Current.Request.QueryString["ID"].ToGuid();
		}

		public static Boolean IsQueryStringInt32(String name)
		{
			String queryVal = GetQueryString(name);
			Int32 output = 0;

			Boolean parsedSuccessfully = Int32.TryParse(queryVal, out output);
			if (!parsedSuccessfully)
				return false;
			else
				return true;

		}

		public static Boolean QueryStringExists(String name)
		{
			String queryVal = HttpContext.Current.Request.QueryString[name];
			if (queryVal == null)
			{
				return false;
			}
			return true;
		}

		public static Int32? GetQueryStringAsInt32OrNull(String name)
		{
			String queryVal = GetQueryString(name);
			Int32 output = 0;

			Boolean parsedSuccessfully = Int32.TryParse(queryVal, out output);
			if (parsedSuccessfully)
			{
				return output;
			}
			return null;
		}

		//public static Guid? GetQueryStringAsGuidOrNull(string name)
		//{
		//    string queryVal = GetQueryString(name);
		//    int output = 0;

		//    bool parsedSuccessfully =  Guid.Parse int.TryParse(queryVal, out output);
		//    if (parsedSuccessfully)
		//        return output;
		//    else
		//        return null;
		//}

		public static Int32? GetID()
		{
			String queryVal = GetQueryString("id");
			Int32 output = 0;

			Boolean parsedSuccessfully = Int32.TryParse(queryVal, out output);
			if (parsedSuccessfully)
			{
				return output;
			}
			return null;
		}

		public static Int32? TryGetQueryStringASInt32(String name)
		{
			String queryVal = GetQueryString(name);
			Int32 output = 0;

			Boolean parsedSuccessfully = Int32.TryParse(queryVal, out output);
			if (!parsedSuccessfully)
			{
				return null;
			}
			return output;
		}

		public static Int32 GetQueryStringASInt32(String name)
		{
			String queryVal = GetQueryString(name);
			Int32 output = 0;

			Boolean parsedSuccessfully = Int32.TryParse(queryVal, out output);
			if (!parsedSuccessfully)
			{
				String errorMessage = $"The query string value '{name}' must be an integer. The provided input was '{queryVal}'.";
				throw new ArgumentException(errorMessage);
			}
			return output;
		}

		public static Boolean QueryStringExistsAndInt32(String name)
		{
			String queryVal = HttpContext.Current.Request.QueryString[name];
			if (queryVal == null)
			{
				return false;
			}
			return IsQueryStringInt32(name);
		}

		public static String GetQueryString(String name)
		{
			String queryVal = HttpContext.Current.Request.QueryString[name];
			if (queryVal == null)
			{
				return String.Empty;
			}
			return queryVal;
		}
	}
}
