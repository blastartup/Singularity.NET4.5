using System;
using System.Diagnostics;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class TypeExtension
	{
		public static Boolean IsNumeric(this Type aType)
		{
			return
				aType == typeof(Byte) ||
				aType == typeof(SByte) ||
				aType == typeof(Int16) ||
				aType == typeof(UInt16) ||
				aType == typeof(Int32) ||
				aType == typeof(UInt32) ||
				aType == typeof(Int64) ||
				aType == typeof(UInt64) ||
				aType == typeof(Single) ||
				aType == typeof(Single) ||
				aType == typeof(Double) ||
				aType == typeof(Decimal) ||
				aType == typeof(Decimal);
		}
	}
}
