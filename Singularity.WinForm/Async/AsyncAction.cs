using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.WinForm.Async
{
	public class AsyncActionExecutor<T>
		where T : BaseView
	{
		public AsyncActionExecutor(T view)
		{
			_view = view;
		}

		public void ExecAysnc(Action<T> asyncAction, AsyncCallback callback)
		{
			asyncAction.BeginInvoke(_view, callback, asyncAction);
		}

		public void ExecAysnc(Action<T, Action<T>> asyncAction, AsyncCallback callback, Action<T> progressChanged)
		{
			asyncAction.BeginInvoke(_view, progressChanged, callback, asyncAction);
		}

		public void ExecAysnc(Action<T, Action<T>, Action<T>> asyncAction, AsyncCallback callback, Action<T> milestoneAchieved, Action<T> completing)
		{
			asyncAction.BeginInvoke(_view, milestoneAchieved, completing, callback, asyncAction);
		}

		private readonly T _view;

		/* Implementation of Callback method:
		 * NB: You must have a callback method as it reduces memory leaks, and traps any exceptions in asyncAction.*/
		//private void Callback(IAsyncResult asyncResult)
		//{
		//	Action<T> asyncAction = (Action<T>)asyncResult.AsyncState;
		//	asyncAction.EndInvoke(asyncResult);
		//}

	}
}
