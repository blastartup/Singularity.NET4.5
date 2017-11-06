using System.Web;
using System.Web.Mvc;

namespace Singularity.Web
{
	public class ImageContentResult : FileContentResult
	{
		public ImageContentResult(byte[] binaryContent, string contentType) : base(binaryContent, contentType)
		{
		}

		public ImageContentResult(CommonHandler commonHandler)
			: base(commonHandler.BinaryContent, commonHandler.ContentType)
		{
			_commonHandler = commonHandler;
		}

		public ImageContentResult(SerialisedFileInfo serialisedFileInfo)
			: base(serialisedFileInfo.SerialisedFile, serialisedFileInfo.FileContentType.ContentType)
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
