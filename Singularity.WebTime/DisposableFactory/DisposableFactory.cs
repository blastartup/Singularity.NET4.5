using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Web;

// ReSharper disable once CheckNamespace

namespace Singularity.WebTime
{
	[DebuggerStepThrough]
	public static class DisposableFactory
	{
		/// <summary>    
		/// Creates a HTTPContext or current thread scoped instance of a disposable object. 
		/// This static method creates a single instance and reuses it whenever this method is    
		/// called from the current Web request.    
		/// </summary>    

		[DebuggerHidden]
		public static TDisposable RequestDisposableObject<TDisposable>(Func<TDisposable> constructor, String instanceKey)
		  where TDisposable : class, IDisposable
		{
			lock (AccessLock)
			{
				TDisposable obj = ContextProvider.GetItem<TDisposable>(instanceKey);
				if (obj != null) return obj;

				obj = constructor();
				ContextProvider.SetItem(instanceKey, obj);
				return obj;
			}
		}

		[DebuggerHidden]
		public static void DisposeObject(String instanceKey)
		{
			lock (AccessLock)
			{
				IDisposable obj = ContextProvider.GetItem<IDisposable>(instanceKey);
				if (obj == null) return;
				obj.Dispose();
				ContextProvider.SetItem<IDisposable>(instanceKey, null);
			}
		}

		[DebuggerHidden]
		static IContextProvider ContextProvider
		{
			get { return _contextProvider ?? (_contextProvider = HttpContext.Current != null ? new HttpContextProvider() : (IContextProvider)new ThreadContextProvider()); }
		}
		private static IContextProvider _contextProvider;

		private static readonly Object AccessLock = new Object();
	}
}
