using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	[DebuggerStepThrough]
	public struct ContentAndFileType : IFileContentType
	{
		public ContentAndFileType(String contentType) : this(contentType, EFileTypes.Unknown)
		{
		}

		public ContentAndFileType(String contentType, EFileTypes eFileType)
		{
			_contentType = contentType;
			_eFileType = eFileType;
		}

		public String ContentType 
		{
			get { return _contentType; }
			set { _contentType = value; }
		}

		private String _contentType;

		public EFileTypes EFileType
		{
			get { return _eFileType; }
			set { _eFileType = value; }
		}

		private EFileTypes _eFileType;
	}
}
