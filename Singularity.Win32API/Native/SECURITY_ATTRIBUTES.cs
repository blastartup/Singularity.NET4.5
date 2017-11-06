using System;
using System.Runtime.InteropServices;

namespace Singularity.Win32API
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct SecurityAttributes
	{
		public Int32 nLength;
		public IntPtr lpSecurityDescriptor;
		public Int32 bInheritHandle;
	}

}
