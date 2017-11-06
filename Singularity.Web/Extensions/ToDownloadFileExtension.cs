using System.IO;
using System.Web.Mvc;

namespace Singularity.Web
{
	public static class ToDownloadFileExtension
	{
		/// <summary>
		/// Converts a string to FileResult for use in MVC Actions using UTF8 encoding.
		/// </summary>
		/// <param name="src">source string</param>
		/// <param name="fileName">The name that will appear in the browser's SaveAs screen</param>
		/// <param name="contentType">The content type to use for the response. defaults to "application/octet-stream" </param>
		/// <returns></returns>
		public static FileResult ToDownloadFile(this string src, string fileName,
										string contentType = System.Net.Mime.MediaTypeNames.Application.Octet)
		{
			var bytes = System.Text.Encoding.UTF8.GetBytes(src);
			return new FileContentResult(bytes, System.Net.Mime.MediaTypeNames.Application.Octet) { FileDownloadName = fileName };
		}

		public static FileResult ToDownloadFile(this Stream src, string fileName,
								string contentType = System.Net.Mime.MediaTypeNames.Application.Octet)
		{
			src.Seek(0, SeekOrigin.Begin);
			return new FileStreamResult(src, System.Net.Mime.MediaTypeNames.Application.Octet) { FileDownloadName = fileName };
		}

		public static FileResult ToDownloadFile(this byte[] src, string fileName,
						string contentType = System.Net.Mime.MediaTypeNames.Application.Octet)
		{
			return new FileContentResult(src, System.Net.Mime.MediaTypeNames.Application.Octet) { FileDownloadName = fileName };
		}
	}
}