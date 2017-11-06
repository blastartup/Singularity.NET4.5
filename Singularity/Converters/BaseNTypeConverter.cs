using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public sealed class BaseNTypeConverter : TypeConverter
	{
		public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object aValue)
		{
			try
			{
				if (aValue is String)
				{
					String lValue = (String)aValue;
					if (!lValue.IsEmpty())
					{
						return new BaseN(Convert.ToInt32(aValue.ToString()));
					}
					return BaseN.Zero;
				}

				if (aValue is Int32 || aValue is Int16 || aValue is Byte || aValue is String)
				{
					return new BaseN(Convert.ToInt32(aValue));
				}

				if (aValue is BaseN)
				{
					return aValue;
				}

				if (aValue == DBNull.Value || aValue == null)
				{
					return BaseN.Zero;
				}
				return base.ConvertFrom(context, culture, aValue);
			}
			catch (FormatException ex)
			{
				throw new FormatException("aValue {0}".FormatX(aValue), ex);
			}
		}

		public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(Int32)
			|| sourceType == typeof(BaseN)
			|| sourceType == typeof(Int16)
			|| sourceType == typeof(Byte)
			|| sourceType == typeof(String)
			|| base.CanConvertFrom(context, sourceType);
		}
	}
}
