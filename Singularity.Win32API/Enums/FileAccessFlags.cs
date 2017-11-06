using System;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	[Flags]
	public enum FileAccessFlags : uint
	{
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000,
		GenericExecute = 0x20000000,
		GenericAll = 0x10000000
	}
}
