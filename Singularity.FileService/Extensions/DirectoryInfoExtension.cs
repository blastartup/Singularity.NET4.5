using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Singularity.FileService
{
	/// <summary>
	/// Extension class for DirectoryInfo
	/// </summary>
	[DebuggerStepThrough]
	public static class DirectoryInfoExtension
	{
		/// <summary>
		/// Return a file count as quickly as possible.
		/// </summary>
		/// <param name="folder">The folder you wish to create.</param>
		/// <returns>Whether or not the folder was created rather than the mere fact the folder exists.</returns>
		public static Int32 Count(this DirectoryInfo folder, String searchPattern, SearchOption searchOption)
		{
			return folder.EnumerateDirectories().AsParallel().SelectMany(d => FastDirectoryEnumerator.EnumerateFiles(folder.FullName, searchPattern, searchOption)).Count();
		}

		/// <summary>
		/// Delete all the files and folders within the current folder but without deleting the current folder itself.
		/// </summary>
		/// <param name="folder">The folder you wish to clean.</param>
		/// <param name="subFoldersOnly">Optionally choose to only delete subfolders and not files in this folder.  By default
		/// files and subfolders are deleted.</param>
		public static void Clean(this DirectoryInfo folder, Boolean subFoldersOnly = false)
		{
			if (!subFoldersOnly)
			{
				folder.GetFiles().ForEach(f => f.Delete());
			}
			folder.GetDirectories().ForEach(f => f.Delete(true));
		}
	}
}
