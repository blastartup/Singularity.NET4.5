using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Singularity
{
	/// <summary>
	/// Extension class for DirectoryInfo
	/// </summary>
	[DebuggerStepThrough]
	public static class DirectoryInfoExtension
	{
		/// <summary>
		/// Create the given folder capturing any exceptions but retrying up to 3 times maximum.
		/// </summary>
		/// <param name="folder">The folder you wish to create.</param>
		/// <returns>Whether or not the folder was created rather than the mere fact the folder exists.</returns>
		public static Boolean CreateSafely(this DirectoryInfo folder)
		{
			Boolean created = false;
			Byte attemptCounter = 0;
			while (!folder.Exists)
			{
				attemptCounter++;
				try
				{
					folder.Create();
					created = true;
					break;
				}
				catch (IOException ex)
				{
					if (attemptCounter < 4)
					{
						if (!ex.Message.ToLower().Contains("network"))
						{
							break;
						}
					}
					else
					{
						throw;
					}
				}
			}
			return created;
		}

		/// <summary>
		/// Delete all the files and folders within the current folder but without deleting the current folder itself.
		/// </summary>
		/// <param name="folder">The folder you wish to clean.</param>
		/// <param name="subFoldersOnly">Optionally choose to only delete sub folders and not files in this folder.  By default
		/// files and sub folders are deleted.</param>
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
