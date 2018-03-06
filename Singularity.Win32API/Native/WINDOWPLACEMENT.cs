using System;

// ReSharper disable once CheckNamespace
namespace Singularity.Win32API
{
	internal struct Windowplacement
	{
		public IntPtr Length;
		public IntPtr Flags;
		public IntPtr ShowCmd;
		public Pointapi PtMinPosition;
		public Pointapi PtMaxPosition;
		public Rect RcNormalPosition;
	}

}
