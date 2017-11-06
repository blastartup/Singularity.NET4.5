using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Singularity.Web
{
	public class ImageFileResult : FilePathResult
	{
		public ImageFileResult(FileInfo imageFileInfo) : base(imageFileInfo.Name, imageFileInfo.GetFileContentType().ContentType)
		{
			_imageFileInfo = imageFileInfo;
		}

		protected override void WriteFile(HttpResponseBase response)
		{
			response.SetDefaultImageHandlers();
			base.WriteFile(response);
		}

		public FileInfo ImageFileInfo
		{
			get { return _imageFileInfo; }
		}
		private readonly FileInfo _imageFileInfo;
	}
}
