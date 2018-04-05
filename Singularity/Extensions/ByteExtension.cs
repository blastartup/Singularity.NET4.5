using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	[DebuggerStepThrough]
	public static class ByteExtension
	{
		public static String ToHexString(this Byte[] byteArray)
		{
			StringBuilder hex = new StringBuilder(byteArray.Length * 2);
			if (byteArray.Length > 0)
			{
				hex.Append("0x");
			}

			foreach (byte b in byteArray)
			{
				hex.AppendFormat("{0:x2}", b);
			}
			return hex.ToString();
		}
	}
}
