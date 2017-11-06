using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface INotification
	{
		String Message { get; set; }
		Exception Exception { get; set; }
	}
}
