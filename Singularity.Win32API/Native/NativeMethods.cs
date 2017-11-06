using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	/// <summary>
	/// NativeWin
	/// </summary>
	internal static class NativeMethods
	{
		#region RPCRT4.dll

		private const Int32 RpcSOk = 0;

		[DllImport("rpcrt4.dll", SetLastError = true)]
		public static extern Int32 UuidCreateSequential(out Guid guid);

		public static Guid CreateSequentialGuid()
		{
			Guid guid;
			Int32 result = UuidCreateSequential(out guid);
			if (result != RpcSOk)
			{
				guid = Guid.NewGuid();
			}
			return guid;
		}

		#endregion

		#region Windows aka User32.dll

		private const Int32 GwHwndfirst = 0;
		private const Int32 GwHwndlast = 1;
		private const Int32 GwHwndnext = 2;
		private const Int32 GwHwndprev = 3;
		private const Int32 GwOwner = 4;
		private const Int32 GwChild = 5;
		private const Int32 GwlExstyle = (-20);
		internal const Int32 HwndBroadcast = 0xffff;
		private const Int32 ScClose = 0xF060;
		internal static IntPtr SwShownormal = new IntPtr(1);
		internal static IntPtr SwShowminimized = new IntPtr(2);
		internal static IntPtr SwShowmaximized = new IntPtr(3);
		private const Int32 WmSyscommand = 0x0112;
		private const Int32 WsExAppwindow = 0x40000;
		private const Int32 WsExToolwindow = 0x80;

		[DllImport("User32.Dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern Int32 FindWindow(String lpClassName, String lpWindowName);

		/// <summary>
		/// Send message to given window.
		/// </summary>
		/// <param name="hWnd">Handle to destination window.</param>
		/// <param name="msg">Message.</param>
		/// <param name="wParam">First message parameter.</param>
		/// <param name="lParam">Second message parameter.</param>
		/// <returns></returns>
		[DllImport("User32.Dll")]
		internal static extern Int32 SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

		[DllImport("User32.Dll")]
		internal static extern Boolean ShowWindow(IntPtr hWnd, IntPtr nCmdShow);

		[DllImport("User32.Dll")]
		internal static extern Boolean ShowWindowAsync(IntPtr hWnd, IntPtr nCmdShow);

		/// <summary>
		/// Set the foreground window.
		/// </summary>
		/// <param name="hWnd">Handle to window</param>
		/// <returns>???</returns>
		[DllImport("User32.Dll")]
		internal static extern Int32 SetForegroundWindow(IntPtr hWnd);

		[DllImport("User32.Dll")]
		internal static extern Int32 EnumWindows(EnumWindowsProcDelegate lpEnumFunc, IntPtr lParam);

		[DllImport("User32.Dll", CharSet = CharSet.Unicode)]
		internal static extern void GetWindowText(IntPtr h, StringBuilder s, IntPtr nMaxCount);

		[DllImport("User32.Dll", EntryPoint = "GetWindowLongA")]
		internal static extern Int32 GetWindowLongPtr(IntPtr hwnd, IntPtr nIndex);

		[DllImport("User32.Dll")]
		internal static extern Int32 GetParent(IntPtr hwnd);

		[DllImport("User32.Dll")]
		internal static extern Int32 GetWindow(IntPtr hwnd, IntPtr wCmd);

		[DllImport("User32.Dll")]
		internal static extern Int32 IsWindowVisible(IntPtr hwnd);

		[DllImport("User32.Dll")]
		internal static extern Int32 GetDesktopWindow();

		[DllImport("User32.Dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern Boolean GetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

		[DllImport("User32.Dll")]
		internal static extern Boolean SetWindowPlacement(IntPtr hWnd, ref Windowplacement lpwndpl);

		[DllImport("User32.Dll")]
		internal static extern Boolean PostMessage(IntPtr hwnd, Int32 msg, IntPtr wparam, IntPtr lparam);

		[DllImport("User32.Dll", CharSet = CharSet.Unicode)]
		internal static extern Int32 RegisterWindowMessage(String message);

		/// <summary>
		/// Enumerate windows processes delegate.
		/// </summary>
		/// <param name="hWnd">Handle to window</param>
		/// <param name="lParam">???</param>
		/// <returns>???</returns>
		internal delegate Int32 EnumWindowsProcDelegate(IntPtr hWnd, IntPtr lParam);

		internal static Int32 RegisterWindowMessage(String format, params Object[] args)
		{
			String message = String.Format(format, args);
			return RegisterWindowMessage(message);
		}

		internal static void WindowAction(String title, IntPtr action)
		{
			IntPtr appHwnd;
			Windowplacement wp = new Windowplacement();
			appHwnd = (IntPtr)FindWindow(null, title);
			GetWindowPlacement(appHwnd, ref wp);
			wp.ShowCmd = action; // 1- Normal; 2 - Minimize; 3 - Maximize;
			SetWindowPlacement(appHwnd, ref wp);
		}

		internal static Int32 ShowToFront(IntPtr window)
		{
			ShowWindow(window, SwShownormal);
			return SetForegroundWindow(window);
		}

		#endregion

		#region IO aka advapi32.dll & kernel32.dll

		internal const Int32 MaxPath = 260;

		// http://msdn.microsoft.com/en-us/library/ms681382(VS.85).aspx
		internal const Int32 ErrorFileNotFound = 2;
		internal const Int32 ErrorNoMoreFiles = 18;
		internal const Int32 OwnerSecurityInformation = 1;
		internal const Int32 SeFileObject = 1;
		//internal static int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		//internal static uint INVALID_FILE_ATTRIBUTES = 0xFFFFFFFF;
		internal static IntPtr InvalidHandleValue = new IntPtr(-1);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern Int32 DuplicateToken(IntPtr hToken, Int32 impersonationLevel, ref IntPtr hNewToken);

		// http://www.dotnet247.com/247reference/msgs/21/108780.aspx
		[DllImportAttribute(@"advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern Int32 GetNamedSecurityInfo(String pObjectName, Int32 objectType, Int32 securityInfo, out IntPtr ppsidOwner, out IntPtr ppsidGroup,
			out IntPtr ppDacl, out IntPtr ppSacl, out IntPtr ppSecurityDescriptor);

		[DllImport("advapi32.dll")]
		public static extern Int32 LogonUserA(String lpszUserName, String lpszDomain, String lpszPassword, Int32 dwLogonType, Int32 dwLogonProvider, ref IntPtr phToken);

		[DllImport(@"advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern Int32 LookupAccountSid(String systemName, IntPtr psid, StringBuilder accountName, ref Int32 cbAccount, [Out] StringBuilder domainName,
			ref Int32 cbDomainName, out Int32 use);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern Boolean RevertToSelf();


		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern Boolean CloseHandle(IntPtr handle);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern Boolean CopyFile([MarshalAs(UnmanagedType.LPTStr)] String lpExistingFileName, [MarshalAs(UnmanagedType.LPTStr)] String lpNewFileName,
							[MarshalAs(UnmanagedType.Bool)] Boolean bFailIfExists);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern Boolean CreateDirectory([MarshalAs(UnmanagedType.LPTStr)]String lpPathName,	IntPtr lpSecurityAttributes);

		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern UInt32 GetFileAttributes([MarshalAs(UnmanagedType.LPTStr)]String lpFileName);

		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern Boolean SetFileAttributes([MarshalAs(UnmanagedType.LPTStr)]String lpFileName, [MarshalAs(UnmanagedType.U4)] AttributeFlags dwFileAttributes);

		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern Boolean RemoveDirectory([MarshalAs(UnmanagedType.LPTStr)]String lpPathName);

		[DllImport(@"kernel32.dll", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern Boolean DeleteFile([MarshalAs(UnmanagedType.LPTStr)]String lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeFindHandle FindFirstFile(String fileName, [In, Out] Win32FindData data);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern Boolean FindNextFile(SafeFindHandle hndFindFile, [In, Out, MarshalAs(UnmanagedType.LPStruct)] Win32FindData lpFindFileData);

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern Boolean FindClose(IntPtr hndFindFile);

		[DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern Int32 GetFullPathName([MarshalAs(UnmanagedType.LPTStr)]String lpFileName, Int32 nBufferLength,	StringBuilder lpBuffer,	IntPtr mustBeZero);

		[DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern SafeFileHandle CreateFile([MarshalAs(UnmanagedType.LPTStr)]String lpFileName, FileAccessFlags dwDesiredAccess, FileShareFlags dwShareMode,
			IntPtr lpSecurityAttributes, CreationDispositionTypes dwCreationDisposition, AttributeFlags dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("KERNEL32.DLL", EntryPoint = "QueryPerformanceFrequency", SetLastError = true,	CharSet = CharSet.Unicode, ExactSpelling = true,
			CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean QueryPerformanceFrequency(ref Int64 frequency);

		[DllImport("KERNEL32.DLL", EntryPoint = "QueryPerformanceCounter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true,
			CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean QueryPerformanceCounter(ref Int64 lpPerformanceCount);

		// Assume dirName passed in is already prefixed with \\?\
		internal static List<String> FindFilesAndDirectories(String directoryPath)
		{
			List<String> results = new List<String>();
			Win32FindData findData = new Win32FindData();
			using (SafeFindHandle findHandle = FindFirstFile(directoryPath.TrimEnd('\\') + @"\*", findData))
			{
				if (!findHandle.IsInvalid)
				{
					Boolean found;
					do
					{
						String currentFileName = findData.cFileName;

						// if this is a directory, find its contents
						if (((Int32)findData.dwFileAttributes & (Int32)AttributeFlags.Directory) != 0)
						{
							if (currentFileName != @"." && currentFileName != @"..")
							{
								List<String> childResults = FindFilesAndDirectories(Path.Combine(directoryPath, currentFileName));
								// add children and self to results
								results.AddRange(childResults);
								results.Add(Path.Combine(directoryPath, currentFileName));
							}
						}

						// it's a file; add it to the results
						else
						{
							results.Add(Path.Combine(directoryPath, currentFileName));
						}

						// find next
						found = FindNextFile(findHandle, findData);
					}
					while (found);
				}

				// close the find handle
				findHandle.Close();
			}
			return results;
		}

		#endregion
	}
}
