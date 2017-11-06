using System;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	public interface IFileContentType
	{
		String ContentType { get; set; }
		EFileTypes EFileType { get; set; }
	}
}
