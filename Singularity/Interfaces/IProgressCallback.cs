using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Interfaces
{
	/// <summary>
	/// Implemented by a form or control, use this interface to asynchronously callback back a progress report.
	/// </summary>
	public interface IProgressCallback
	{
		void Begin();
		void Begin(Int32 minium, Int32 maximum);
		void SetRange(Int32 minimum, Int32 maximum);
		void SetText(String text);
		void Increment(Int32 value = 1);
		Boolean IsAborting { get; }
		void End();
	}
}
