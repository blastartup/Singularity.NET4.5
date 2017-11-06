using System.IO;
using System.Web;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class HttpPostedFileExtension
	{
		public static FileInfo PostedFileInfo(this HttpPostedFile postedFile)
		{
			return new FileInfo(postedFile.FileName);
		}
	}
}
