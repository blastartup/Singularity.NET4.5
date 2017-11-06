using System;
using System.IO;
using System.Threading;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	public class SerialisedFileInfo : IStateValid, IStateEmpty
	{
		public SerialisedFileInfo(Stream fileStream, FileInfo fileInfo)
		{
			if (fileStream == null || fileInfo == null)
			{
				throw new InvalidOperationException("Cannot serialise a null stream and/or file.");
			}

			this.fileInfo = fileInfo;
			this.fileStream = fileStream;
			SerialisedFileInfoCore(fileStream, fileInfo);
		}

		protected virtual void SerialisedFileInfoCore(Stream fileStream, FileInfo fileInfo)
		{
			name = fileInfo.Name;
			fileContentType = fileInfo.GetFileContentType();
			length = fileStream.Length <= Int32.MaxValue ? (Int32)fileStream.Length : 0;
		}

		public Stream FileStream
		{
			get { return fileStream; }
			set { fileStream = value; }
		}
		protected Stream fileStream;

		public FileInfo FileInfo
		{
			get { return fileInfo; }
			set { fileInfo = value; }
		}
		protected FileInfo fileInfo;

		public String Name
		{
			get { return name; }
		}
		protected String name;

		public IFileContentType FileContentType
		{
			get { return fileContentType; }
		}
		protected IFileContentType fileContentType;

		public Int32 Length
		{
			get { return length; }
		}
		protected Int32 length;

		public virtual Boolean IsValid
		{
			get { return !IsEmpty && fileStream != null && fileInfo != null; }
		}

		public Boolean IsEmpty
		{
			get { return length.IsEmpty(); } 
		}

		public virtual Byte[] SerialisedFile
		{
			get
			{
				if (Content == null && IsValid)
				{
					try
					{
						Content = new Byte[(Int32)fileStream.Length];
						fileStream.Read(Content, 0, length);
						Thread.Sleep(0);
					}
					catch (Exception)
					{
						Content = null;
					}
				}
				return Content;
			}
		}

		protected Byte[] Content;

		public static explicit operator FileContent(SerialisedFileInfo serialisedFileInfo)
		{
			return new FileContent(serialisedFileInfo);
		}
	}
}
