using System;
using System.IO;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Simple attribute class for storing String Values
	/// </summary>
	/// <remarks>Either needs to be used internally only, or if to be visible to users,
	/// it needs to return a culture specific string based on the given enum code - hence the name of the attribute.</remarks>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class EnumAdditionalAttribute : Attribute, IComparable, IComparable<EnumAdditionalAttribute>
	{
		/// <summary>
		/// Creates a new <see cref="EnumAdditionalAttribute"/> instance.
		/// </summary>
		/// <param name="humanisedName">A humanised name or heading to assign to Enum.</param>
		/// <param name="code">A code to assign to Enum.</param>
		/// <param name="description">Describe the Enum.</param>
		/// <param name="keyGuid">A primary key (Guid) as a string matching the equivelant enum in the database type table.</param>
		/// <param name="alternateValue">An alternate value other than the enum numerical one.</param>
		public EnumAdditionalAttribute(String humanisedName, String code = null, String description = null, String keyGuid = null, String alternateValue = null)
		{
			_humanisedName = humanisedName;
			_code = code;
			_description = description;
			_alternateValue = alternateValue;

			if (!keyGuid.IsEmpty() && keyGuid.IsGuid())
			{
				_key = keyGuid.ToGuid();
			}
		}

		/// <summary>
		/// Gets the enum Code.
		/// </summary>
		public String AlternateValue => _alternateValue;
		private readonly String _alternateValue;

		/// <summary>
		/// Gets the enum Code.
		/// </summary>
		public String Code => _code;
		private readonly String _code;

		/// <summary>
		/// Gets the enum Description.
		/// </summary>
		public String Description => _description;
		private readonly String _description;

		/// <summary>
		/// Gets the enum Name.
		/// </summary>
		public String HumanisedName => _humanisedName;
		private readonly String _humanisedName;

		/// <summary>
		/// Gets the enum primary Key.
		/// </summary>
		public Guid? Key => _key;
		private readonly Guid? _key;

		/// <summary>
		/// Get the enum integer.
		/// </summary>
		public Int32 EnumValue
		{
			get => _enumValue;
			internal set => _enumValue = value;
		}
		private Int32 _enumValue;

		public override Boolean Equals(Object obj)
		{
			return ReferenceEquals(this, obj);
		}

		public Int32 GetHashCode(EnumAdditionalAttribute obj)
		{
			return obj?.GetHashCode() ?? 0;
		}

		public override Int32 GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				Int32 hash = (Int32)2166136261;
				// Suitable nullity checks etc, of course :)
				hash = hash * 16777619 ^ _alternateValue.GetHashCode();
				hash = hash * 16777619 ^ _code.GetHashCode();
				hash = hash * 16777619 ^ _description.GetHashCode();
				hash = hash * 16777619 ^ _humanisedName.GetHashCode();
				hash = hash * 16777619 ^ _key.GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Compare to another EnumAdditionalAttribute
		/// </summary>
		/// <param name="other">Given EnumAdditionalAttribute to compare to.</param>
		/// <returns>Standard comparison values.</returns>
		public Int32 CompareTo(EnumAdditionalAttribute other)
		{
			return String.Compare(HumanisedName, other.HumanisedName, StringComparison.Ordinal);
		}

		/// <summary>
		/// Compare to another Object
		/// </summary>
		/// <param name="obj">Given Object to compare to.</param>
		/// <returns>Standard comparison values.</returns>
		public Int32 CompareTo(Object obj)
		{
			EnumAdditionalAttribute attribute = obj as EnumAdditionalAttribute;
			return attribute != null ? String.Compare(HumanisedName, attribute.HumanisedName, StringComparison.Ordinal) : 0;
		}
	}
}
