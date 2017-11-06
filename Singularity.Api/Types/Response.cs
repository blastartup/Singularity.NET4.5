using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace Singularity.Api
{
	/// <summary>
	/// A class to give us a neat response. Can have great extensibility in future.
	/// </summary>
	[CLSCompliant(true)]
	public class Response<T> where T : DtoBase
	{
		public Response()
		{ }

		public Response(List<T> collection, Int32 itemCount)
		{
			Collection = collection;
			ItemCount = itemCount;
		}

		public List<T> Collection { get; set; }

		public Int32 ItemCount { get; set; }

	}
}