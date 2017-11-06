using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.Api
{
	/// <summary>
	/// Mirror to an API end point a non-String data type as a string and optionally which format to use.
	/// </summary>
	public enum EControllerStringTypeAndFormat
	{
		/// <summary>
		/// Pass through the original data type and do not convert to a string.
		/// </summary>
		[EnumAdditional("Pass Through")]
		PassThrough,

		/// <summary>
		/// Convert a non-String data type to a native formatted string data type.
		/// </summary>
		[EnumAdditional("Plain Conversion")]
		Plain,

		/// <summary>
		/// Convert usually a decimal data type to a formatted currency string data type.
		/// </summary>
		[EnumAdditional("Currency Conversion", alternateValue:".FormatCurrency()")]
		Currency,

		/// <summary>
		/// Convert a native DateTime data type to a formatted short date string data type.
		/// </summary>
		[EnumAdditional("Localised Short Date Conversion", alternateValue: ".ToDateTime(\"s\")")]
		LocalShortDate,
	}
}
