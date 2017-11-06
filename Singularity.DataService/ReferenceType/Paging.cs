using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	[DebuggerStepThrough]
	public sealed class Paging
	{
		public Paging(Int32 take) : this(take, 1) { }

		public Paging(Int32 pageSize, Int32 pageNumber)
		{
			PageSize = pageSize;
			_pageNumber = pageNumber;
		}

		public Int32 PageSize
		{
			get { return _pageSize; }
			set { _pageSize = !value.IsEmpty() ? value : DefaultTake; }
		}

		private Int32 _pageSize;

		public Int32 PageNumber
		{
			get { return _pageNumber; }
			set { _pageNumber = value; }
		}

		private Int32 _pageNumber;


		public Int32 Take
		{
			get { return _pageSize; }
		}

		public Int32 Skip
		{
			get { return (_pageNumber - 1) * _pageSize; }
		}

		private const Int32 DefaultTake = 1000;
	}
}