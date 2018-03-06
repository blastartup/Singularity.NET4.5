
// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum EMemoryUomTypes : byte
	{
		[EnumAdditional("b", "bit")]
		bit,
		[EnumAdditional("B", "Byte")]
		B,
		[EnumAdditional("KB", "KiloByte")]
		KB,
		[EnumAdditional("MB", "MegaByte")]
		MB,
		[EnumAdditional("GB", "GigaByte")]
		GB,
		[EnumAdditional("TB", "TerraByte")]
		TB
	}
}
