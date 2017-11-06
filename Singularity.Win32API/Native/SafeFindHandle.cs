using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Singularity.Win32API
{
	/// <summary>
	/// Wraps a FindFirstFile handle.
	/// </summary>
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SafeFindHandle"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFindHandle() : base(true) { }

		/// <summary>
		/// When overridden in a derived class, executes the code required to free the handle.
		/// </summary>
		/// <returns>
		/// true if the handle is released successfully; otherwise, in the 
		/// event of a catastrophic failure, false. In this case, it 
		/// generates a releaseHandleFailed MDA Managed Debugging Assistant.
		/// </returns>
		protected override Boolean ReleaseHandle()
		{
			return NativeMethods.FindClose(handle);
		}
	}
}
