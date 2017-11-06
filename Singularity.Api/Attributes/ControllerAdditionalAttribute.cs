using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace

namespace Singularity.Api
{
	/// <summary>
	/// Am attribute to implement or overrule how a property is genertated in a Dto and/or used by a Controller class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ControllerAdditionalAttribute : Attribute
	{
		/// <summary>
		/// Primary constructor with all arguments set by default.
		/// </summary>
		/// <param name="eControllerAccess">Override the default behaviour of the property to be generated or not in a Dto.</param>
		/// <param name="eControllerSearchable">Indicate whether the property is searchable.</param>
		/// <param name="eControllerStringTypeAndFormat">Flag to mirror the original data type as a string.</param>
		public ControllerAdditionalAttribute(EControllerAccess eControllerAccess = EControllerAccess.ReadWrite, 
			EControllerSearchable eControllerSearchable = EControllerSearchable.Not,
			EControllerStringTypeAndFormat eControllerStringTypeAndFormat = EControllerStringTypeAndFormat.PassThrough)
		{
			EControllerAccess = eControllerAccess;
			EControllerSearchable = eControllerSearchable;
			EControllerStringTypeAndFormat = eControllerStringTypeAndFormat;
		}

		/// <summary>
		/// Override a NonMapped or Computed column to be visible, or a normal property (perhaps an obsolete one) to be hidden.
		/// </summary>
		public EControllerAccess EControllerAccess { get; set; }

		/// <summary>
		/// Indicate if the property can be searched - affects the end point signature, Controller and Service classes.
		/// </summary>
		public EControllerSearchable EControllerSearchable { get; set; }

		/// <summary>
		/// Override the default data type as String and a string format.
		/// </summary>
		public EControllerStringTypeAndFormat EControllerStringTypeAndFormat { get; set; }
	}
}
