using System;
using System.IO;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Singularity.Win32API
{
	/// <summary>
	/// Contains information about the file that is found 
	/// by the FindFirstFile or FindNextFile functions.
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode), BestFitMapping(false)]
	internal struct Win32FindData
	{
		internal FileAttributes dwFileAttributes;
		[NonSerialized]
		internal Filetime ftCreationTime;
		[NonSerialized]
		internal Filetime ftLastAccessTime;
		[NonSerialized]
		internal Filetime ftLastWriteTime;
		internal UInt32 nFileSizeHigh;
		internal UInt32 nFileSizeLow;
		internal Int32 dwReserved0;
		internal Int32 dwReserved1;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.MaxPath)]
		internal String cFileName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		internal String cAlternateFileName;

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override String ToString()
		{
			return "File name=" + cFileName;
		}
	}
}
