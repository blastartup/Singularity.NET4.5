using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Singularity
{
	/// <summary>
	/// Extension to Object Class
	/// </summary>
#if !DEBUG
	[DebuggerStepThrough]
#endif
	public static class ObjectExtension
	{
		/// <summary>
		/// Is value, enumerable or collection null, empty or min value?
		/// </summary>
		/// <param name="value">Value to determine emptiness.</param>
		/// <returns>True if value is empty.</returns>
		[DebuggerStepThrough]
		[Pure]
		public static Boolean IsEmpty(this Object value)
		{
			Boolean result = false;

			if (value == null)
			{
				result = true;
			}
			else if (value is String)
			{
				result = String.IsNullOrEmpty(value as String);
			}
			else if (value is Int32)
			{
				result = ((Int32)value).Equals(0);
			}
			else if (value is DateTime)
			{
				result = ((DateTime)value).Equals(DateTime.MinValue) || ((DateTime)value).Equals(DateTimeExtension.MinJsonValue);
			}
			else if (value is TimeSpan)
			{
				result = ((TimeSpan)value).Equals(TimeSpan.MinValue);
			}
			else if (value is Guid)
			{
				result = ((Guid)value).Equals(Guid.Empty);
			}
			else if (value is Decimal)
			{
				result = ((Decimal)value).Equals(0);
			}
			else if (value is Double)
			{
				result = ((Double)value).Equals(0);
			}
			else if (value is Single)
			{
				result = ((Single)value).Equals(0);
			}
			else if (value is Char)
			{
				result = ((Char)value).Equals('\0');
			}
			else if (value is IEnumerable)
			{
				Boolean allItemsEmpty = true;
				foreach (Object item in (IEnumerable)value)
				{
					allItemsEmpty &= item.IsEmpty();

					if (!allItemsEmpty)
					{
						break;
					}
				}
				result = allItemsEmpty;
			}
			else if (value is IStateEmpty)
			{
				result = ((IStateEmpty)value).IsEmpty;
			}

			return result;
		}

		/// <summary>
		/// Output the value of this object to a humanised form.
		/// </summary>
		/// <param name="value">Any given object.</param>
		/// <returns>Either a string representation of the object value, "Empty", "Null", or "DBNull".</returns>
		[DebuggerStepThrough]
		public static String ToDescription(this Object value)
		{
			String result;
			if (value == null)
			{
				result = ValueLib.NullDescription.StringValue;
			}
			else if (value is DBNull)
			{
				result = ValueLib.DbNullDescription.StringValue;
			}
			else if (IsStateEmpty(value as IStateEmpty))
			{
				result = ValueLib.EmptyDescription.StringValue;
			}
			else if (value is IStateValid && !((IStateValid)value).IsValid)
			{
				result = ValueLib.InvalidDescription.StringValue;
			}
			else if (value is IStateAged && !((IStateAged)value).IsAged)
			{
				result = ValueLib.NotAgedDescription.StringValue;
			}
			else if (value is TimeSpan)
			{
				result = TimeSpanArticulator.Articulate((TimeSpan)value);
			}
			else if (value is Type)
			{
				result = ((Type)value).Name;
			}
			else if (value is IDictionary)
			{
				IDictionary dictionary = value as IDictionary;
				DelimitedStringBuilder innerResult = new DelimitedStringBuilder(dictionary.Count);
				foreach (Object key in dictionary.Keys)
				{
					innerResult.Add("{0}{1}{2}", key.ToDescription(), ValueLib.KeyValueDelimiter.StringValue, dictionary[key].ToDescription());
				}
				result = innerResult.ToDelimitedString(ValueLib.ValueMark.CharValue);
			}
			else
			{
				result = value.ToString();
			}
			return result;
		}

		private static Boolean IsStateEmpty(IStateEmpty stateEmpty)
		{
			return stateEmpty != null && stateEmpty.IsEmpty;
		}

		[DebuggerStepThrough]
		public static Boolean In<T>(this T aObject, params T[] values)
		{
			return values.Contains(aObject);
		}

		[DebuggerStepThrough]
		public static Boolean In<T>(this T aObject, IEnumerable<T> valueList)
		{
			return valueList.Contains(aObject);
		}

		[DebuggerStepThrough]
		public static TObject Swap<TObject>(this TObject value1, ref TObject value2)
		{
			TObject result = value2;
			value2 = value1;
			return result;
		}

		#region ToX()

		[DebuggerStepThrough]
		public static String ToStringSafe(this Object value)
		{
			return ToStringSafe(value, String.Empty);
		}

		[DebuggerStepThrough]
		public static String ToStringSafe(this Object value, String replacementValue)
		{
			return value?.ToString() ?? replacementValue;
		}

		#region ToInt

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <returns>Either the correctly converted Int32 or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Int32 ToInt(this Object value)
		{
			return ToInt(value, 0);
		}

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Int32 or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Int32 ToInt(this Object value, Int32 defaultValue)
		{
			return (Int32)ToCore(value, defaultValue);
		}

		#endregion

		#region ToInt64

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <returns>Either the correctly converted Int32 or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Int64 ToInt64(this Object value)
		{
			return ToInt64(value, 0);
		}

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Int32 or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Int64 ToInt64(this Object value, Int64 defaultValue)
		{
			return (Int64)ToCore(value, defaultValue);
		}

		#endregion

		#region ToBool

		/// <summary>
		/// Exception protected Object to bool conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected boolean value.</param>
		/// <returns>Either the correctly converted bool or false if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Boolean ToBool(this Object value)
		{
			return ToBool(value, false);
		}

		/// <summary>
		/// Exception protected Object to bool conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected boolean value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted bool or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Boolean ToBool(this Object value, Boolean defaultValue)
		{
			return (Boolean)ToCore(value, defaultValue);
		}

		#endregion

		#region ToDecimal

		/// <summary>
		/// Exception protected Object to Decimal conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Decimal value.</param>
		/// <returns>Either the correctly converted Decimal or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Decimal ToDecimal(this Object value)
		{
			return ToDecimal(value, 0m);
		}

		/// <summary>
		/// Exception protected Object to Decimal conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Decimal value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Decimal or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Decimal ToDecimal(this Object value, Decimal defaultValue, NumberStyles? numberStyles = null)
		{
			return (Decimal)ToCore(value, defaultValue, numberStyles);
		}

		#endregion

		#region ToDouble

		/// <summary>
		/// Exception protected Object to Double conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Double value.</param>
		/// <returns>Either the correctly converted Double or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Double ToDouble(this Object value)
		{
			return ToDouble(value, 0);
		}

		/// <summary>
		/// Exception protected Object to Double conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Double value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Double or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Double ToDouble(this Object value, Double defaultValue)
		{
			return (Double)ToCore(value, defaultValue);
		}

		#endregion

		#region ToDateTime

		/// <summary>
		/// Exception protected Object to DateTime conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected DateTime value.</param>
		/// <returns>Either the correctly converted DateTime or DateTime.MinValue if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static DateTime ToDateTime(this Object value)
		{
			return ToDateTime(value, DateTime.MinValue);
		}

		/// <summary>
		/// Exception protected Object to DateTime conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected DateTime value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted DateTime or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static DateTime ToDateTime(this Object value, DateTime defaultValue)
		{
			return (DateTime)ToCore(value, defaultValue);
		}

		#endregion

		#region ToGuid

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <returns>Either the correctly converted Int32 or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Guid ToGuid(this Object value)
		{
			return ToGuid(value, Guid.Empty);
		}

		/// <summary>
		/// Exception protected Object to Int32 conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected integer value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Int32 or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Guid ToGuid(this Object value, Guid defaultValue)
		{
			return (Guid)ToCore(value, defaultValue);
		}

		#endregion

		#region ToSingle

		/// <summary>
		/// Exception protected Object to Double conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Double value.</param>
		/// <returns>Either the correctly converted Double or 0 if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Single ToSingle(this Object value)
		{
			return ToSingle(value, 0);
		}

		/// <summary>
		/// Exception protected Object to Double conversion.
		/// </summary>
		/// <param name="value">Given Object as an expected Double value.</param>
		/// <param name="defaultValue">Default value if the object is null or the conversion fails.</param>
		/// <returns>Either the correctly converted Double or the default value if the given value is invalid in any way.</returns>
		[DebuggerStepThrough]
		public static Single ToSingle(this Object value, Single defaultValue)
		{
			return (Single)ToCore(value, defaultValue);
		}

		#endregion

		private static Object ToCore(Object value, Object defaultValue, NumberStyles? numberStyles = null)
		{
			Object result = defaultValue;
			if (value != null)
			{
				try
				{
					if (result is Int64)
					{
						result = Convert.ToInt64(value);
					}
					else if (result is Int32)
					{
						result = Convert.ToInt32(value);
					}
					else if (result is Decimal)
					{
						if (numberStyles == null)
						{
							result = Convert.ToDecimal(value);
						}
						else if (value is String)
						{
							result = Decimal.Parse(value.ToString(), numberStyles.Value);
						}
					}
					else if (result is Boolean)
					{
						result = Convert.ToBoolean(value);
					}
					else if (result is DateTime)
					{
						result = ConvertToDateTime(value, (DateTime)defaultValue);
					}
					else if (result is Double)
					{
						result = Convert.ToDouble(value);
					}
					else if (result is Guid)
					{
						if (value is String)
						{
							result = new Guid((String)value);
						}
						else if (value is Byte[])
						{
							result = new Guid((Byte[])value);
						}
						else if (value is Guid)
						{
							result = value;
						}
					}
					else if (result is Single)
					{
						result = Convert.ToSingle(value);
					}
				}
				catch (FormatException) { }
				catch (InvalidCastException) { }
			}
			return result;
		}

		private static DateTime ConvertToDateTime(Object value, DateTime defaultValue)
		{
			DateTime result = defaultValue;
			try
			{
				result = Convert.ToDateTime(value);
			}
			catch (FormatException) { }
			catch (InvalidCastException) { }
			return result;
		}

		#endregion

		#region ToNullableX()

		#region ToNullableInt()

		[DebuggerStepThrough]
		public static Int32? ToNullableInt(this Object value)
		{
			return ToNullableInt(value, null);
		}

		[DebuggerStepThrough]
		public static Int32? ToNullableInt(this Object value, Int32? defaultValue)
		{
			Int32? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToInt(defaultValue.Value) : value.ToInt();
			}
			return result;
		}

		#endregion

		#region ToNullableInt64()

		[DebuggerStepThrough]
		public static Int64? ToNullableInt64(this Object value)
		{
			return ToNullableInt64(value, null);
		}

		[DebuggerStepThrough]
		public static Int64? ToNullableInt64(this Object value, Int64? defaultValue)
		{
			Int64? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToInt64(defaultValue.Value) : value.ToInt();
			}
			return result;
		}

		#endregion

		#region ToNullableDateTime()

		[DebuggerStepThrough]
		public static DateTime? ToNullableDateTime(this Object value)
		{
			return ToNullableDateTime(value, null);
		}

		[DebuggerStepThrough]
		public static DateTime? ToNullableDateTime(this Object value, DateTime? defaultValue)
		{
			DateTime? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToDateTime(defaultValue.Value) : value.ToDateTime();
			}
			return result;
		}

		#endregion

		#region ToNullableDecimal()

		[DebuggerStepThrough]
		public static Decimal? ToNullableDecimal(this Object value)
		{
			return ToNullableDecimal(value, null);
		}

		[DebuggerStepThrough]
		public static Decimal? ToNullableDecimal(this Object value, Decimal? defaultValue)
		{
			Decimal? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToDecimal(defaultValue.Value) : value.ToDecimal();
			}
			return result;
		}

		#endregion

		#region ToNullableBoolean()

		[DebuggerStepThrough]
		public static Boolean? ToNullableBoolean(this Object value)
		{
			return ToNullableBoolean(value, null);
		}

		public static Boolean? ToNullableBoolean(this Object value, Boolean? defaultValue)
		{
			Boolean? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToBool(defaultValue.Value) : value.ToBool();
			}
			return result;
		}

		#endregion

		#region ToNullableDouble()

		[DebuggerStepThrough]
		public static Double? ToNullableDouble(this Object value)
		{
			return ToNullableDouble(value, null);
		}

		[DebuggerStepThrough]
		public static Double? ToNullableDouble(this Object value, Double? defaultValue)
		{
			Double? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToDouble(defaultValue.Value) : value.ToDouble();
			}
			return result;
		}

		#endregion

		#region ToNullableSingle()

		[DebuggerStepThrough]
		public static Single? ToNullableSingle(this Object value)
		{
			return ToNullableSingle(value, null);
		}

		[DebuggerStepThrough]
		public static Single? ToNullableSingle(this Object value, Single? defaultValue)
		{
			Single? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToSingle(defaultValue.Value) : value.ToSingle();
			}
			return result;
		}

		#endregion

		#region ToNullableGuid()

		[DebuggerStepThrough]
		public static Guid? ToNullableGuid(this Object value)
		{
			return ToNullableGuid(value, null);
		}

		[DebuggerStepThrough]
		public static Guid? ToNullableGuid(this Object value, Guid? defaultValue)
		{
			Guid? result = defaultValue;
			if (value != null)
			{
				result = defaultValue != null ? value.ToGuid(defaultValue.Value) : value.ToGuid();
			}
			return result;
		}

		#endregion

		#endregion

		#region KeyValuePairs

		[DebuggerStepThrough]
		public static KeyValuePairs PropertiesToKeyValuePairs<TObject>(this TObject @object) where TObject : class
		{
			KeyValuePairs result = new KeyValuePairs();

			if (@object != null)
			{
				AddFields(@object).ForEach(p => result.Add(p));
				AddProperties(@object).ForEach(p => result.Add(p));
			}
			return result;
		}

		[DebuggerStepThrough]
		private static KeyValuePairs AddFields<TObject>(TObject @object, Int32 nestCounter = 0) where TObject : class
		{
			KeyValuePairs result = new KeyValuePairs();
			if (@object != null && nestCounter < MaxNesting)
			{
				nestCounter++;
				FieldInfo[] fields = @object.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo field in fields)
				{
					Object valueObject = field.GetValue(@object);
					KeyValuePair<String, String> keyValuePair = GetKeyValue(field.Name, valueObject);
					if (!keyValuePair.Key.IsEmpty())
					{
						result.Add(keyValuePair);
					}
					else if (valueObject is IList)
					{
						Int32 itemCntr = 0;
						foreach (Object item in (IList)valueObject)
						{
							result.Add(new KeyValuePair<String, String>("{0}[{1}]".FormatX(field.Name, itemCntr++), AddProperties(item, nestCounter).ToString()));
						}
					}
					else
					{
						result.Add(new KeyValuePair<String, String>(field.Name, AddProperties(valueObject, nestCounter).ToString()));
					}
				}
			}
			return result;
		}

		private static KeyValuePairs AddProperties<TObject>(TObject @object, Int32 nestCounter = 0) where TObject : class
		{
			KeyValuePairs result = new KeyValuePairs();
			if (@object != null && nestCounter < MaxNesting)
			{
				nestCounter++;
				List<PropertyInfo> properties = @object.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p != null).ToList();
				foreach (PropertyInfo property in properties)
				{
					Object valueObject = property.GetValue(@object, null);
					if (valueObject == null)
					{
						continue;
					}

					KeyValuePair<String, String> keyValuePair = GetKeyValue(property.Name, valueObject);
					if (!keyValuePair.Key.IsEmpty())
					{
						result.Add(keyValuePair);
					}
					else if (valueObject is IList)
					{
						Int32 itemCntr = 0;
						foreach (Object item in (IList)valueObject)
						{
							result.Add(new KeyValuePair<String, String>("{0}[{1}]".FormatX(property.Name, itemCntr++), AddProperties(item, nestCounter).ToString()));
						}
					}
					else
					{
						result.Add(new KeyValuePair<String, String>(property.Name, AddProperties(valueObject, nestCounter).ToString()));
					}
				}
			}
			return result;
		}

		private const Int32 MaxNesting = 5;

		private static KeyValuePair<String, String> GetKeyValue(String fieldName, Object valueObject)
		{
			KeyValuePair<String, String> result = new KeyValuePair<String, String>();
			if (valueObject != null)
			{
				Type valueObjectType = valueObject.GetType();
				if (valueObjectType.Name == "String")
				{
					result = new KeyValuePair<String, String>(fieldName, valueObject.ToString());
				}
				else if (valueObjectType.IsValueType)
				{
					result = new KeyValuePair<String, String>(fieldName, GetStringValue(valueObject, valueObjectType));
				}
				else if (valueObjectType.Name == "Byte[]")
				{
					result = new KeyValuePair<String, String>(fieldName, ByteValueFormat.FormatX(((Byte[])valueObject).Length));
				}
			}
			return result;
		}

		private const String ByteValueFormat = "byte[{0}]";

		private static String GetStringValue(Object valueObject, Type valueObjectType)
		{
			String result = String.Empty;
			if (valueObjectType == typeof(DateTime))
			{
				result = ((DateTime)valueObject).ToString("dd/MM/yyyy");
			}
			else if (valueObjectType == typeof(Boolean))
			{
				result = ((Boolean)valueObject) ? "Yes" : "No";
			}
			else
			{
				result = valueObject.ToString();
			}
			return result;
		}

		#endregion

		public static TValue ReplaceUsing<TValue>(this TValue value, IEnumerable<Tuple<TValue, TValue>> replacementValueMaps)
		{
			foreach (Tuple<TValue, TValue> replacementValueMap in replacementValueMaps)
			{
				if (replacementValueMap.Item1.Equals(value))
				{
					return replacementValueMap.Item2;
				}
			}
			return value;
		}

		public static ExpandoObject ToExpandoObject(this Object[] objects)
		{
			Contract.Requires(objects != null);

			ExpandoObject model = new ExpandoObject();
			if (objects != null)
			{
				IDictionary<String, Object> modelDictionary = model as IDictionary<String, Object>;
				ToExpandoObjectCore(objects, modelDictionary);
			}
			return model;
		}

		private static void ToExpandoObjectCore(Object[] objects, IDictionary<String, Object> modelDictionary)
		{
			foreach (Object o in objects)
			{
				if (o == null)
				{
					continue;
				}

				if (o is IEnumerable)
				{
					ToExpandoObjectCore(o as Object[], modelDictionary);
					continue;
				}

				PropertyInfo[] properties = o.GetType().GetProperties();
				foreach (PropertyInfo property in properties)
				{
					modelDictionary[property.Name] = property.GetValue(o, null);
				}
			}
		}

		/// <summary>
		/// Obtain the value of a property by supplying it's name as a string.
		/// </summary>
		/// <param name="model">Object with properties</param>
		/// <param name="propertyName">Name of the property whose value you want.</param>
		/// <returns></returns>
		public static Object GetPropertyValue(this Object model, String propertyName)
		{
			String varName = propertyName;
			Int32 indexOfDot = propertyName.IndexOf('.');
			while (indexOfDot != -1)
			{
				String currentObjectName = varName.Substring(0, indexOfDot);
				varName = varName.Substring(indexOfDot + 1);
				indexOfDot = varName.IndexOf('.');
				model = model.GetPropertyValue(currentObjectName);
				if (model == null) return propertyName; //if not found - return unchanged 
			}

			// ExpandoObject support
			Object dv = TryGetDictionaryValue<Object>(model, varName);
			if (dv != null) return dv;

			// any IDictionary<String,String> support
			dv = TryGetDictionaryValue<String>(model, varName);
			if (dv != null) return dv;

			Object pv = TryGetPropertyValue(model, varName);
			return pv ?? propertyName;

			//no fields - discourage bad design
			//var field = type.GetField(name);
			//if (field != null) return field.GetValue(obj);
		}

		private static Object TryGetDictionaryValue<TValue>(Object obj, String name) where TValue : class
		{
			IDictionary<String, TValue> dicObj = obj as IDictionary<String, TValue>;
			if (dicObj != null)
			{
				return dicObj.ContainsKey(name) ? dicObj[name] : null;
			}
			return null;
		}

		private static Object TryGetPropertyValue(Object obj, String name)
		{
			Type type = obj.GetType();
			PropertyInfo property = type.GetProperty(name);
			if (property != null) return property.GetValue(obj, null);
			return null;
		}

	}
}

