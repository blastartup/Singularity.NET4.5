using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Singularity.Win32API
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Filetime
	{
		internal UInt32 dwLowDateTime;
		internal UInt32 dwHighDateTime;
	}
}
