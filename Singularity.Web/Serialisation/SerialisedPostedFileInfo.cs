using System;
using System.IO;
using System.Net;
using System.Web;

namespace Singularity.Web
{
	public class SerialisedPostedFileInfo : SerialisedFileInfo
	{
		public SerialisedPostedFileInfo(Stream fileStream, FileInfo fileInfo) : base(fileStream, fileInfo)
		{
		}

		public SerialisedPostedFileInfo(HttpPostedFile postedFile)
			: base(postedFile.InputStream, postedFile.PostedFileInfo())
		{
			fileContentType = postedFile.PostedFileInfo().GetFileContentType();
			if (fileContentType == null)
			{
				fileContentType = new ContentAndFileType()
				{
					ContentType = postedFile.ContentType,
					EFileType = EFileType.Document,
				};
			}
			length = postedFile.ContentLength;
		}

		public static SerialisedFileInfo GetSerialisedPostedFileInfo(String url)
		{
			HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
			WebResponse myResp = myReq.GetResponse();
			Stream stream = myResp.GetResponseStream();
			FileInfo fileInfo = new FileInfo(Path.GetFileName(url));

			return new SerialisedFileInfo(stream, fileInfo);
		}

		protected override void SerialisedFileInfoCore(Stream fileStream, FileInfo fileInfo)
		{
			// do nothing.
		}


	}
}
