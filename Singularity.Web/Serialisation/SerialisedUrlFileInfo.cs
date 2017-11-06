using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Singularity.Web
{
	public class SerialisedUrlFileInfo : SerialisedFileInfo
	{
		protected SerialisedUrlFileInfo(Stream fileStream, FileInfo fileInfo)
			: base(fileStream, fileInfo)
		{
		}

		public static SerialisedFileInfo New(String url)
		{
			HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
			WebResponse myResp = myReq.GetResponse();
			Stream stream = myResp.GetResponseStream();
			FileInfo fileInfo = new FileInfo(Path.GetFileName(url));

			return new SerialisedUrlFileInfo(stream, fileInfo);
		}

		protected override void SerialisedFileInfoCore(Stream fileStream, FileInfo fileInfo)
		{
			name = fileInfo.Name;
			fileContentType = fileInfo.GetFileContentType();
		}

		public override Byte[] SerialisedFile
		{
			get
			{
				if (content == null && IsValid)
				{
					try
					{
						using (BinaryReader br = new BinaryReader(fileStream))
						{
							content = br.ReadBytes(500000);
						}
						length = content.Length;
						Thread.Sleep(0);
					}
					catch (Exception)
					{
						content = null;
					}
				}
				return content;
			}
		}

		public override bool IsValid
		{
			get { return fileStream != null && fileInfo != null; }
		}
	}
}
