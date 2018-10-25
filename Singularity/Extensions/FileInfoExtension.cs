using System;
using System.Diagnostics;
using System.IO;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Extension class for the FileInfo.
	/// </summary>
#if !DEBUG
	[DebuggerStepThrough]
#endif
	public static class FileInfoExtension
	{
		public static Boolean CopyToIfRequired(this FileInfo sourceFile, FileInfo targetFile)
		{
			Byte retryCounter = 0;
			while (!File.Exists(targetFile.FullName) && retryCounter < 3)
			{
				try
				{
					sourceFile.CopyTo(targetFile.FullName);
				}
				catch (IOException ex)
				{
					if (!ex.Message.ToLower().Contains("network"))
					{
						break;
					}
				}
				retryCounter++;
			}
			return File.Exists(targetFile.FullName);
		}

		/// <summary>
		/// Return the filename without the file extension.
		/// </summary>
		/// <param name="sourceFileInfo">The source FileInfo from which to obtain the Name.</param>
		/// <returns>A name without an extension.</returns>
		public static String NameSansExtension(this FileInfo sourceFileInfo)
		{
			return sourceFileInfo.Name.Cut(ValueLib.FullStop.CharValue, false);
		}

		public static void SetInCurrentDirectoryIfRequired(this FileInfo sourceFileInfo)
		{
			if (sourceFileInfo.FullName.SubstringSafe(1, 1) != ValueLib.Colon.StringValue)
			{
				sourceFileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, sourceFileInfo.FullName));
			}
		}
	}
}
