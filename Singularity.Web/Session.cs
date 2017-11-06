using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Singularity.Web
{
	public static class SessionUtil
	{
		public static String DetermineIP(HttpContext context)
		{
			IList<String> allKeys = context.Request.ServerVariables.AllKeys;

			if (allKeys.Contains("HTTP_CLIENT_IP") && CheckIp(context.Request.ServerVariables["HTTP_CLIENT_IP"]))
			{
				return context.Request.ServerVariables["HTTP_CLIENT_IP"];
			}

			if (allKeys.Contains("HTTP_X_FORWARDED_FOR"))
			{
				foreach (String ip in context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(','))
				{
					if (CheckIp(ip.Trim()))
					{
						return ip.Trim();
					}
				}
			}

			if (allKeys.Contains("HTTP_X_FORWARDED") && CheckIp(context.Request.ServerVariables["HTTP_X_FORWARDED"]))
			{
				return context.Request.ServerVariables["HTTP_X_FORWARDED"];
			}

			if (allKeys.Contains("HTTP_X_CLUSTER_CLIENT_IP") && CheckIp(context.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"]))
			{
				return context.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];
			}

			if (allKeys.Contains("HTTP_FORWARDED_FOR") && CheckIp(context.Request.ServerVariables["HTTP_FORWARDED_FOR"]))
			{
				return context.Request.ServerVariables["HTTP_FORWARDED_FOR"];
			}

			if (allKeys.Contains("HTTP_FORWARDED") && CheckIp(context.Request.ServerVariables["HTTP_FORWARDED"]))
			{
				return context.Request.ServerVariables["HTTP_FORWARDED"];
			}

			return context.Request.ServerVariables["REMOTE_ADDR"];
		}

		private static Boolean CheckIp(String ip)
		{
			if (!String.IsNullOrEmpty(ip))
			{
				Int64 ipToLong = -1;
				//Is it valid IP address
				if (TryConvertIPToLong(ip, out ipToLong))
				{
					//Does it fall within a private network range
					return _privateIps.All(privateIp => (ipToLong < privateIp[0]) || (ipToLong > privateIp[1]));
				}
				return false;
			}
			return false;
		}

		private static Boolean TryConvertIPToLong(String ip, out Int64 ipToLong)
		{
			try
			{
				ipToLong = ConvertIPToLong(ip);
				return true;
			}
			catch
			{
				ipToLong = -1;
				return false;
			}
		}

		private static Int64 ConvertIPToLong(String ip)
		{
			String[] ipSplit = ip.Split('.');
			return (16777216 * Convert.ToInt32(ipSplit[0]) + 65536 * Convert.ToInt32(ipSplit[1]) + 256 * Convert.ToInt32(ipSplit[2]) + Convert.ToInt32(ipSplit[3]));
		}


		private static Int64[][] _privateIps = new Int64[][] {
		  new Int64[] {ConvertIPToLong("0.0.0.0"), ConvertIPToLong("2.255.255.255")},
		  new Int64[] {ConvertIPToLong("10.0.0.0"), ConvertIPToLong("10.255.255.255")},
		  new Int64[] {ConvertIPToLong("127.0.0.0"), ConvertIPToLong("127.255.255.255")},
		  new Int64[] {ConvertIPToLong("169.254.0.0"), ConvertIPToLong("169.254.255.255")},
		  new Int64[] {ConvertIPToLong("172.16.0.0"), ConvertIPToLong("172.31.255.255")},
		  new Int64[] {ConvertIPToLong("192.0.2.0"), ConvertIPToLong("192.0.2.255")},
		  new Int64[] {ConvertIPToLong("192.168.0.0"), ConvertIPToLong("192.168.255.255")},
		  new Int64[] {ConvertIPToLong("255.255.255.0"), ConvertIPToLong("255.255.255.255")}
		};

		public static String GetSession(String name)
		{
			if (SessionExists(name))
			{
				return HttpContext.Current.Session[name].ToString();
			}
			return String.Empty;
		}

		public static Boolean SessionExists(String name)
		{
			if (HttpContext.Current.Session[name] == null)
			{
				return false;
			}
			return true;
		}

		private static Int32? GetInt32(String sessionName)
		{
			if (SessionExists(sessionName))
			{
				Int32 temp;
				if (Int32.TryParse(GetSession(sessionName), out temp))
				{
					return temp;
				}
				return null;
			}
			return null;
		}

		public static Int32 AccountID
		{
			get
			{
				if (HttpContext.Current.Session["AccountID"] != null)
				{
					return (Int32)HttpContext.Current.Session["AccountID"];
				}
				return -1;
			}
			set
			{
				HttpContext.Current.Session["AccountID"] = value;
			}
		}

		public static Boolean IsAuthorised(Int32 checkPointID)
		{
			return AuthorisedCheckPoints != null && AuthorisedCheckPoints.Contains(checkPointID);
		}

		public static List<Int32> AuthorisedCheckPoints
		{
			get
			{
				if (HttpContext.Current.Session["AuthorisedCheckPoints"] != null)
				{
					return (List<Int32>)HttpContext.Current.Session["AuthorisedCheckPoints"];
				}
				return null;
			}
			set
			{
				HttpContext.Current.Session["AuthorisedCheckPoints"] = value;
			}
		}

		public static Int32 HeadOfficeID
		{
			get
			{
				if (HttpContext.Current.Session["HeadOfficeID"] != null)
				{
					return (Int32)HttpContext.Current.Session["HeadOfficeID"];
				}
				return -1;
			}
			set
			{
				HttpContext.Current.Session["HeadOfficeID"] = value;
			}
		}
	}
}
