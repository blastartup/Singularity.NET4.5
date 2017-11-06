using System;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	[Flags]
	public enum CopyToFlags : short
	{
		Default = 0,
		OverwriteExisting = 1,
		PreserveFileDateTime = 2
	}
}
