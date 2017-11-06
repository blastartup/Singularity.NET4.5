using System;
using System.Collections.Generic;
using System.Reflection;

namespace Singularity
{
	public class EnumAdditionalProvider<T> where T : Attribute
	{
		public T GetEnumResource(Enum value)
		{
			T result = null;
			Type lType = value.GetType();

			if (EnumResourceAttributeCollection.ContainsKey(value))
			{
				result = (EnumResourceAttributeCollection[value] as T);
			}
			else
			{
				//Look for our 'StringValueAttribute' in the field's custom attributes
				FieldInfo lFieldInfo = lType.GetField(value.ToString());
				if (lFieldInfo == null)
				{
					throw new InvalidOperationException("Cannot find an EnumAdditional attribute for value '{0}' in enum type {1}.".FormatX(value, typeof(T)));
				}
				T[] lAttributes = lFieldInfo.GetCustomAttributes(typeof(T), false) as T[];
				if (lAttributes?.Length > 0)
				{
					EnumResourceAttributeCollection.Add(value, lAttributes[0]);
					result = lAttributes[0];
				}
			}
			return result;
		}

		private Dictionary<Enum, T> EnumResourceAttributeCollection
		{
			get { return _mEnumResourceAttributeCollection ?? (_mEnumResourceAttributeCollection = new Dictionary<Enum, T>()); }
		}

		private Dictionary<Enum, T> _mEnumResourceAttributeCollection;
	}
}
