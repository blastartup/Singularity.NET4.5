using System;
using System.Diagnostics;
using System.IO;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	[DebuggerStepThrough]
	public struct FileContent : IFileContent
	{
		public FileContent(SerialisedFileInfo serialisedFileInfo)
		{
			_contentId = Guid.Empty;
			_originalFileName = serialisedFileInfo.FileInfo.Name;
			_displayName = Path.GetFileNameWithoutExtension(serialisedFileInfo.FileInfo.Name);
			_description = String.Empty;
			_binaryContent = serialisedFileInfo.SerialisedFile;
			_length = serialisedFileInfo.Length;
			_fileContentType = serialisedFileInfo.FileContentType;
			_expiryDate = null;
		}

		public Guid ContentId
		{
			get { return _contentId; }
			set { _contentId = value; }
		}
		private Guid _contentId;

		public String OriginalFileName
		{
			get { return _originalFileName; }
			set { _originalFileName = value; }
		}
		private String _originalFileName;

		public String DisplayName
		{
			get { return _displayName; }
			set { _displayName = value; }
		}
		private String _displayName;

		public String Description
		{
			get { return _description; }
			set { _description = value; }
		}
		private String _description;

		public Int32? Length
		{
			get { return _length; }
			set { _length = value; }
		}
		private Int32? _length;

		public Byte[] BinaryContent
		{
			get { return _binaryContent; }
			set { _binaryContent = value; }
		}
		private Byte[] _binaryContent;

		public IFileContentType FileContentType
		{
			get { return _fileContentType; }
			set { _fileContentType = value; }
		}
		private IFileContentType _fileContentType;

		public DateTime? ExpiryDate => _expiryDate;
		private DateTime? _expiryDate;

		public Boolean IsValid => !IsEmpty && _binaryContent != null && !_originalFileName.IsEmpty();

		public Boolean IsEmpty => _length.IsEmpty();

		// NB: Once IsTemp is true, it cannot be made false.  A temp FileContent must be cloned or deleted.
		public Boolean IsTemp => _expiryDate != null;

		public void SetAsTemporary()
		{
			if (_expiryDate == null)
			{
				SetAsTemporary(DateTime.Now.AddMinutes(30));
			}
		}

		public void SetAsTemporary(DateTime expiryDate)
		{
			if (_expiryDate == null)
			{
				_expiryDate = expiryDate;
				_displayName = _originalFileName;
				_originalFileName = Guid.NewGuid().ToString();
			}
		}

		public IFileContent ToPermanentFileContent()
		{
			return new FileContent()
			{
				ContentId = Guid.Empty,
				OriginalFileName = this.OriginalFileName,
				DisplayName = this.DisplayName,
				Description = this.Description,
				Length = this.Length,
				BinaryContent = this.BinaryContent,
				FileContentType = this.FileContentType,
			};
		}
	}
}
