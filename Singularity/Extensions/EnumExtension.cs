using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Extensions for Enums.
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		/// Gets a Guid primary key for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>Guid primary key value associated via a attribute, or null if not found.</returns>
		public static Guid? GetKey(this Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			return provider.GetEnumResource(enumValue)?.Key;
		}

		/// <summary>
		/// Gets a Code for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		public static String GetAlternateValue(this Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			return provider.GetEnumResource(enumValue)?.AlternateValue;
		}

		/// <summary>
		/// Gets a Code for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		public static String GetCode(this Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			return provider.GetEnumResource(enumValue)?.Code ?? String.Empty;
		}

		/// <summary>
		/// Gets the Humanised Name for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		public static String GetHumanisedName(this Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			return provider.GetEnumResource(enumValue)?.HumanisedName ?? Enum.GetName(enumValue.GetType(), enumValue) ?? String.Empty;
		}

		///// <summary>
		///// Gets a Value Name for a particular enum value.
		///// </summary>
		///// <param name="enumValue">Enum value</param>
		///// <returns>String Value Name associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		//public static String GetValueName(this Enum enumValue)
		//{
		//	Contract.Requires(enumValue != null);

		//	var provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
		//	return provider.GetEnumResource(enumValue)?.ValueName ?? String.Empty;
		//}

		/// <summary>
		/// Gets a Description for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		public static String GetDescription(this Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			EnumAdditionalAttribute resource = provider.GetEnumResource(enumValue);
			return resource != null ? resource.Description : Enum.GetName(enumValue.GetType(), enumValue);
		}

		/// <summary>
		/// Gets the first non empty value of either Code, Name or Description for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="EnumAdditionalAttribute"/> attribute, or null if not found.</returns>
		public static String GetCodeNameOrDescription(this Enum enumValue)
		{
			if (enumValue == null)
			{
				throw new ArgumentNullException(nameof(enumValue));
			}

			String result;
			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			EnumAdditionalAttribute resource = provider.GetEnumResource(enumValue);
			if (resource != null)
			{
				if (!resource.Code.IsEmpty())
				{
					result = resource.Code;
				}
				else if (!resource.HumanisedName.IsEmpty())
				{
					result = resource.HumanisedName;
				}
				else if (!resource.Description.IsEmpty())
				{
					result = resource.Description;
				}
				else
				{
					result = Enum.GetName(enumValue.GetType(), enumValue);
				}
			}
			else
			{
				result = enumValue.ToString();
			}
			return result;
		}

	}
}
