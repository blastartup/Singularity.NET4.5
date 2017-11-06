using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.WinForm
{
	[DebuggerStepThrough]
	internal class ThreadContextProvider : IContextProvider
	{
		[DebuggerHidden]
		public T GetItem<T>(String key) where T : class
		{
			LocalDataStoreSlot lds = Thread.GetNamedDataSlot(key);
			return (T)Thread.GetData(lds);
		}

		[DebuggerHidden]
		public void SetItem<T>(String key, T value) where T : class
		{
			LocalDataStoreSlot lds = Thread.GetNamedDataSlot(key);
			Thread.SetData(lds, value);
		}
	}
}
