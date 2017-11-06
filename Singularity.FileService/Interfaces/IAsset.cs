using System;

// ReSharper disable once CheckNamespace

namespace Singularity.FileService
{
	public interface IAsset
	{
		Guid ContentId { get; set; }
		Byte[] BinaryContent { get; set; }
		String DisplayName { get; set; }
		Int32? Length { get; set; }
	}
}
