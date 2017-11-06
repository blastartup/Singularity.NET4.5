using System;
using System.Runtime.InteropServices;

namespace Singularity.Win32API
{
	public class SafeNativeMethods
	{
		// WinNT.h
		public const Int32 DELETE = 0x00010000;
		public const Int32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;

		// WinSvc.h
		public const Int32 SC_MANAGER_CONNECT = 0x0001;
		public const Int32 SC_MANAGER_CREATE_SERVICE = 0x0002;
		public const Int32 SC_MANAGER_ENUMERATE_SERVICE = 0x0004;
		public const Int32 SC_MANAGER_LOCK = 0x0008;
		public const Int32 SC_MANAGER_QUERY_LOCK_STATUS = 0x0010;
		public const Int32 SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020;

		public const Int32 SC_MANAGER_ALL_ACCESS =
			STANDARD_RIGHTS_REQUIRED |
			SC_MANAGER_CONNECT |
			SC_MANAGER_CREATE_SERVICE |
			SC_MANAGER_ENUMERATE_SERVICE |
			SC_MANAGER_LOCK |
			SC_MANAGER_QUERY_LOCK_STATUS |
			SC_MANAGER_MODIFY_BOOT_CONFIG
			;

		// WinError.h
		public const Int32 ERROR_SERVICE_DOES_NOT_EXIST = 1060;
		public const Int32 ERROR_SERVICE_MARKED_FOR_DELETE = 1072;

		public virtual IntPtr OpenSCManager(String machineName, String databaseName, Int32 access)
		{
			return PInvokeDeclarations.OpenSCManager(machineName, databaseName, access);
		}

		public virtual IntPtr OpenService(IntPtr databaseHandle, String serviceName, Int32 access)
		{
			return PInvokeDeclarations.OpenService(databaseHandle, serviceName, access);
		}
 
		public virtual Boolean CloseServiceHandle(IntPtr handle)
		{
			return PInvokeDeclarations.CloseServiceHandle(handle);
		}

		public virtual Boolean DeleteService(IntPtr serviceHandle)
		{
			return PInvokeDeclarations.DeleteService(serviceHandle);
		}
		
		// Declarations were copied from decompiled .NET Framework assemblies
		class PInvokeDeclarations
		{
			[DllImportAttribute("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
			public static extern IntPtr OpenSCManager(String machineName, String databaseName, Int32 access);

			[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
			public static extern IntPtr OpenService(IntPtr databaseHandle, String serviceName, Int32 access);
 
			[DllImportAttribute("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
			public static extern Boolean CloseServiceHandle(IntPtr handle);

			[DllImport("advapi32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
			public static extern Boolean DeleteService(IntPtr serviceHandle);
		}
	}
}
