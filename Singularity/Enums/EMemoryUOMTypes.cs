
// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum EMemoryUomTypes : byte
	{
		[EnumAdditional("b", "bit")]
		b,
		[EnumAdditional("B", "Byte")]
		B,
		[EnumAdditional("KB", "KiloByte")]
		Kb,
		[EnumAdditional("MB", "MegaByte")]
		Mb,
		[EnumAdditional("GB", "GigaByte")]
		Gb,
		[EnumAdditional("TB", "TerraByte")]
		Tb
	}
}
