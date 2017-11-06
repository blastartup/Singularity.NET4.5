using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Singularity.Web
{
	public class ImageStreamResult : FileStreamResult
	{
		public ImageStreamResult(CommonHandler commonHandler)
			: base(new MemoryStream(commonHandler.BinaryContent), commonHandler.ContentType)
		{
			_commonHandler = commonHandler;
		}

		public ImageStreamResult(SerialisedFileInfo serialisedFileInfo)
			: base(serialisedFileInfo.FileStream, serialisedFileInfo.FileContentType.ContentType)
		{
			_serialisedFileInfo = serialisedFileInfo;
		}

		protected override void WriteFile(HttpResponseBase response)
		{
			response.SetDefaultImageHandlers();
			base.WriteFile(response);
		}

		private readonly SerialisedFileInfo _serialisedFileInfo;
		private readonly CommonHandler _commonHandler;
	}
}
