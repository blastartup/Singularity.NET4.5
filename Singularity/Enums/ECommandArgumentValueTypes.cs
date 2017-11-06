
namespace Singularity
{
	/// <summary>
	/// Defines the type of possible commandline arguments.
	/// Note all arguments can either start with / or with -.
	/// </summary>
	public enum ECommandArgumentValueTypes
	{
		/// <summary>
		/// <para>Parameter must be followed by a string,</para>
		/// examples: /p string, /p two string, /p "enclosed by quotes", /p "enclosed by single quotes"
		/// </summary>
		String,
		/// <summary>
		/// <para>Paramter can be followed by a string,</para>
		/// examples: /p, /p string
		/// </summary>
		OptionalString,
		/// <summary>
		/// <para>Parameter must be following be a whole number. Note: Negativ numbers must be quoted!</para>
		/// Examples: /p 12, /p "-100"
		/// </summary>
		Int,
		/// <summary>
		/// <para>Parameter can be followed by a whole number. Note: Negativ numbers must be quoted!</para>
		/// <para>If no number is specified it will be interpreted as 0.</para>
		/// Examples: /p, /p 12, /p "-100"
		/// </summary>
		OptionalInt,
		/// <summary>
		/// <para>Parameter is a switch. It can be defined or not.</para>
		/// <para>Note: If AllowType is set to Required, this parameter kind does not make much sense!</para>
		/// Examples: program.exe, program.exe /p
		/// </summary>
		Bool,
		/// <summary>
		/// <para>Parameter can be used many times, however they must not be a value defined.</para>
		/// <para>The number of times of occurances is count (/p /p /p will return "3").</para>
		/// <para>Note: If AllowType is set to Required, switch must appear at least once!</para>
		/// Examples: program.exe, program.exe /p, program.exe /p /p /p /p
		/// </summary>
		MultipleBool,
		/// <summary>
		/// <para>Parameter can be followed by several whole numbers.</para>
		/// <para>If AllowType is set to Required, at least one number must be specified.</para>
		/// <para>Note: Inclusing a negativ number requires quoting the series of numbers.</para>
		/// Examples: /p 1, /p 1 2 3, /p "0 1 -1 2 -2"
		/// </summary>
		MultipleInts
	};
}
