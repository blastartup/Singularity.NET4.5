using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public static class EnumUtil
	{
		#region EnumAdditional implementation

		public static IList<Guid?> GetKeys(Type enumType)
		{
			List<Guid?> result = new List<Guid?>();
			result.AddRange(GetEnumsCore(enumType).Select(e => e.Key));
			return result;
		}

		/// <summary>
		/// Get Guids for the supplied enum vaues
		/// </summary>
		/// <param name="enumValues">enum values</param>
		/// <returns></returns>
		public static List<Guid?> GetKeys(params Enum[] enumValues)
		{
			return enumValues.Select(EnumExtension.GetKey).ToList();
		}

		public static IList<String> GetCodes(Type enumType)
		{
			List<String> result = new List<String>();
			result.AddRange(GetEnumsCore(enumType).Select(e => e.Code));
			return result;
		}

		public static IList<String> GetHumanisedNames(params Enum[] enumValues)
		{
			List<String> result = new List<String>(enumValues.Length);
			enumValues.ForEach(ev => result.Add(ev.GetHumanisedName()));
			return result;
		}

		public static IList<String> GetHumanisedNames(Type enumType)
		{
			List<String> result = new List<String>();
			result.AddRange(GetEnumsCore(enumType).Select(e => e.HumanisedName));
			return result;
		}

		public static IList<String> GetDescriptions(Type enumType)
		{
			List<String> result = new List<String>();
			result.AddRange(GetEnumsCore(enumType).Select(e => e.Description));
			return result;
		}

		public static IList<EnumAdditionalAttribute> GetEnumAdditionals(Type enumType)
		{
			return GetEnumsCore(enumType);
		}

		private static IList<EnumAdditionalAttribute> GetEnumsCore(Type enumType)
		{
			List<EnumAdditionalAttribute> result = new List<EnumAdditionalAttribute>();
			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fieldInfo in enumType.GetFields())
			{
				//Check for our custom attribute
				EnumAdditionalAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumAdditionalAttribute), false) as EnumAdditionalAttribute[];
				if (attributes != null && attributes.Length > 0)
				{
					EnumAdditionalAttribute attribute = attributes[0];
					attribute.EnumValue = (Int32)fieldInfo.GetValue(enumType);
					//attribute.ValueName = fieldInfo.Name;
					result.Add(attribute);
				}
			}
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
		public static Boolean ParseCode(Type type, String code, Boolean ignoreCase, ref Object value)
		{
			Object defaultValue = (value != null) ? value : null;
			Object returnValue = ParseCode(type, code, ignoreCase);
			if (returnValue == null && defaultValue != null)
			{
				value = defaultValue;
			}
			else
			{
				value = returnValue;
			}
			return value != null;
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value (case sensitive).
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static Object ParseCode(Type type, String code)
		{
			return ParseCode(type, code, false);
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="stringValue">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static Object ParseCode(Type type, String code, Boolean ignoreCase)
		{
			Contract.Requires(type.IsEnum);
			Contract.Requires(!String.IsNullOrEmpty(code));

			Object lOutput = null;
			String lEnumStringCode = String.Empty;

			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				//Check for our custom attribute
				EnumAdditionalAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumAdditionalAttribute), false) as EnumAdditionalAttribute[];
				if (attributes.Length > 0)
				{
					lEnumStringCode = attributes[0].Code;

					//Check for equality then select actual enum value.
					if (String.Compare(lEnumStringCode, code, ignoreCase) == 0)
					{
						lOutput = Enum.Parse(type, fieldInfo.Name);
						break;
					}
				}
			}
			return lOutput;
		}

		/// <summary>
		/// Parses the supplied enum and string value to find an associated enum value.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="enumName">String value.</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Enum value associated with the string value, or null if not found.</returns>
		public static Object ParseName(Type type, String enumName)
		{
			Contract.Requires(type.IsEnum);
			Contract.Requires(!String.IsNullOrEmpty(enumName));

			return (from name in Enum.GetNames(type)
					  where enumName.Equals(name, StringComparison.OrdinalIgnoreCase)
					  select Enum.Parse(type, name)).FirstOrDefault();
		}

		/// <summary>
		/// Gets an enum for a particular enum value.
		/// </summary>
		/// <param name="enumValue">Enum value</param>
		/// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
		public static Int32 GetEnumInt(Enum enumValue)
		{
			Contract.Requires(enumValue != null);

			Int32 result = 0;
			EnumAdditionalProvider<EnumAdditionalAttribute> provider = new EnumAdditionalProvider<EnumAdditionalAttribute>();
			EnumAdditionalAttribute resource = provider.GetEnumResource(enumValue);
			if (resource != null)
			{
				result = resource.EnumValue;
			}
			return result;
		}

		/// <summary>
		/// Given a Guid primary key, return the associated enum for it.
		/// </summary>
		/// <param name="enumType">Enum type the primary key ia associated with.</param>
		/// <param name="Guid">Primary key for the enum table type.</param>
		/// <returns>Enum value associated with the primary key, or null if not found.</returns>
		public static Object GetEnum(Type enumType, Guid key)
		{
			Contract.Requires(enumType.IsEnum);
			Contract.Requires(!key.IsEmpty());

			Object lOutput = null;
			Guid? lEnumKey = null;

			//Look for our string value associated with fields in this enum
			foreach (FieldInfo fieldInfo in enumType.GetFields())
			{
				//Check for our custom attribute
				EnumAdditionalAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EnumAdditionalAttribute), false) as EnumAdditionalAttribute[];
				if (attributes.Length > 0)
				{
					lEnumKey = attributes[0].Key;

					//Check for equality then select actual enum value.
					if (lEnumKey == key)
					{
						lOutput = Enum.Parse(enumType, fieldInfo.Name);
						break;
					}
				}
			}
			return lOutput;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <returns>Existence of the string value</returns>
		public static Boolean IsStringDefined(Type enumType, String stringValue)
		{
			return ParseCode(enumType, stringValue) != null;
		}

		/// <summary>
		/// Return the existence of the given string value within the enum.
		/// </summary>
		/// <param name="stringValue">String value.</param>
		/// <param name="enumType">Type of enum</param>
		/// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
		/// <returns>Existence of the string value</returns>
		public static Boolean IsStringDefined(Type enumType, String stringValue, Boolean ignoreCase)
		{
			return ParseCode(enumType, stringValue, ignoreCase) != null;
		}

		#endregion

		/// <summary>
		/// Loops over each item in the enum type and returns the key and value of each.
		/// </summary>
		public static List<KeyValuePair<Int32, String>> GetEnumValues(Type enumType)
		{
			List<KeyValuePair<Int32, String>> returnValue = new List<KeyValuePair<Int32, String>>();

			IList<EnumAdditionalAttribute> enumAdditionalList = GetEnumAdditionals(enumType);
			foreach (EnumAdditionalAttribute enumAdditional in enumAdditionalList)
			{
				returnValue.Add(new KeyValuePair<Int32, String>(enumAdditional.EnumValue, enumAdditional.HumanisedName));
			}
			return returnValue;
		}
	}
}
