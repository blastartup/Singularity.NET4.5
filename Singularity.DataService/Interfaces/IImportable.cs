using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IImportable : IModifiable
	{
		DateTime? ImportedDate { get; set; }
	}
}
