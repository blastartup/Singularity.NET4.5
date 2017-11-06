// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	#region Objects needed for the Win32 functions
	#pragma warning disable 1591

	/// <summary>
	/// The resource type.
	/// </summary>
	public enum ResourceType : int
	{
		Any = 0,
		Disk = 1,
		Print = 2,
		Reserved = 8,
	}

	#pragma warning restore 1591
	#endregion
 }