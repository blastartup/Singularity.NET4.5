using System;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	public interface IFileContent : IAsset, IStateEmpty, IStateValid
	{
		String OriginalFileName { get; set; }
		String Description { get; set; }
		DateTime? ExpiryDate { get; }
		Boolean IsTemp { get; }
		IFileContentType FileContentType { get; set; }

		void SetAsTemporary();
		void SetAsTemporary(DateTime expiryDate);
		IFileContent ToPermanentFileContent();
	}
}
