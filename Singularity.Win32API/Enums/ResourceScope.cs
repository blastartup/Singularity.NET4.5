// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	#region Objects needed for the Win32 functions
	#pragma warning disable 1591

	/// <summary>
	/// The resource scope.
	/// </summary>
	public enum ResourceScope : int
	{
		Connected = 1,
		GlobalNetwork,
		Remembered,
		Recent,
		Context
	};

	#pragma warning restore 1591
	#endregion
}