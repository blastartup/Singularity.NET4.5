using System;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	[Flags]
	public enum FileShareFlags : uint
	{
		None = 0x00000000,
		Read = 0x00000001,
		Write = 0x00000002,
		Delete = 0x00000004,
	}
}
