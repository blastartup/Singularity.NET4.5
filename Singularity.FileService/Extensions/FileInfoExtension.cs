using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	/// <summary>
	/// Extension class for the FileInfo.
	/// </summary>
	public static class FileInfoExtension
	{
		public static async Task CopyToAsync(this FileInfo sourceFileInfo, FileInfo targetFileInfo)
		{
			using (FileStream sourceStream = System.IO.File.Open(sourceFileInfo.FullName, FileMode.Open))
			{
				using (FileStream destinationStream = System.IO.File.Create(targetFileInfo.FullName))
				{
					await sourceStream.CopyToAsync(destinationStream);
				}
			}
		}

		public static async Task WriteAllTextAsync(this FileInfo fileInfo, String text)
		{
			Byte[] plainText = text.ToByteArray();

			using (FileStream sourceStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None,
				 4096, true))
			{
				await sourceStream.WriteAsync(plainText, 0, plainText.Length);
			};
		}

		public static Task WriteAllTextTaskAsync(this FileInfo fileInfo, String text, out FileStream sourceStream)
		{
			Byte[] plainText = text.ToByteArray();

			sourceStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None,
				4096, true);
			return sourceStream.WriteAsync(plainText, 0, plainText.Length);
		}

		public static async Task<String> ReadAllTextAsync(this FileInfo fileInfo)
		{
			using (FileStream sourceStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read,
				 4096, true))
			{
				StringBuilder sb = new StringBuilder();

				Byte[] buffer = new Byte[0x1000];
				Int32 numRead;
				while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
				{
					String text = Encoding.Unicode.GetString(buffer, 0, numRead);
					sb.Append(text);
				}

				return sb.ToString();
			}

		}

		// Todo - replace switch table by looking up a T4 auto generated enum from a MimeType reference table.
		public static IFileContentType GetFileContentType(this FileInfo documentFile)
		{
			ContentAndFileType result = new ContentAndFileType();
			switch (documentFile.Extension.ToLower())
			{
				case ".doc":
				case ".docx":
					result.ContentType = "application/msword";
					result.EFileType = EFileTypes.Document;
					break;
				case ".jpg":
				case ".jpeg":
					result.ContentType = "image/jpeg";
					result.EFileType = EFileTypes.Image;
					break;
				case ".png":
					result.ContentType = "image/png";
					result.EFileType = EFileTypes.Image;
					break;
				case ".gif":
					result.ContentType = "image/gif";
					result.EFileType = EFileTypes.Image;
					break;
				case ".bmp":
					result.ContentType = "image/bmp";
					result.EFileType = EFileTypes.Image;
					break;
				case ".tif":
				case ".tiff":
					result.ContentType = "image/tiff";
					result.EFileType = EFileTypes.Document;
					break;
				case ".pdf":
					result.ContentType = "application/pdf";
					result.EFileType = EFileTypes.Document;
					break;
				case ".swf":
					result.ContentType = "application/x-shockwave-flash";
					result.EFileType = EFileTypes.Video;
					break;
				case ".xml":
					result.ContentType = "application/xml";
					result.EFileType = EFileTypes.Document;
					break;
			}
			return result;
		}

		public static IFileContent ToFileContent(this FileInfo fileInfo)
		{
			return new FileContent(new SerialisedFileInfo(fileInfo.OpenRead(), fileInfo));
		}

		public static Boolean IsValidFilename(this FileInfo fileInfo)
		{
			return fileInfo.Name.IsValidFilename();
		}

		public static Boolean IsFileLocked(this FileInfo fileinfo)
		{
			FileStream stream = null;
			try
			{
				stream = fileinfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				stream?.Close();
			}
			return false;
		}
	}
}
