using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	#region Objects needed for the Win32 functions
	#pragma warning disable 1591

	/// <summary>
	/// The net resource.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class NetResource
	{
		public ResourceScope Scope;
		public ResourceType ResourceType;
		public ResourceDisplaytype DisplayType;
		public Int32 Usage;
		public String LocalName;
		public String RemoteName;
		public String Comment;
		public String Provider;
	}

	#pragma warning restore 1591
	#endregion
}