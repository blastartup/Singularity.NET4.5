
 // ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Capacity Types for the FList generic object.  These types set the default capacity for the generic list to minimise memory swapping as the list grows incrementally.
	/// </summary>
	public enum EListCapacityType
	{
		[EnumAdditional("NoCapacity")]
		None = 0,
		[EnumAdditional("MinimumCapacity")]
		Minimum = 10,
		[EnumAdditional("MedimumCapacity")]
		Medium = 100,
		[EnumAdditional("MaximumCapacity")]
		Maximum = 1000
	}
}
