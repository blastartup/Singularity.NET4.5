using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Be able to rewind or stop the clock by replacing DateTime.UtcNow.
	/// </summary>
	/// <remarks>
	/// One can WindBackTo() a specified date time in the past, and each call to SystemClock.Now will return the DateTime starting from that point
	/// in the past.  The clock will then run double speed until it catches up with real time.<br/>
	/// <br/>
	/// Alternatively one can Stop() the clock.  However to maintain chronological order, each subsequent call to SystemClock.Now, will advance the
	/// clock by a single tick.<br/>
	/// <br/>
	/// Start()'ing the clock will start the clock and revert back to the WindBackTo(current clock date time) process.<br/>
	/// <br/>
	/// Obviously for a web service, the entire scope of the system clock is for the duration of the web request, and will not affect other web requests.
	/// </remarks>
	public abstract class SystemClock
	{
		public void StopClock()
		{
			if (!_stopClock)
			{
				_stopClock = true;
				_ignitionPoint = DateTime.UtcNow;
			}
		}

		public void StartClock()
		{
			if (_stopClock)
			{
				_stopClock = false;
				_reflectionSpan = DateTime.UtcNow - _ignitionPoint;
				_ignitionPoint = DateTime.UtcNow;
			}
		}

		public void Set(DateTime reflectionPoint)
		{
			Contract.Assert(reflectionPoint < DateTime.UtcNow);

			_ignitionPoint = reflectionPoint;
			_reflectionSpan = _ignitionPoint - reflectionPoint;
		}
		private TimeSpan? _reflectionSpan;
		private DateTime _ignitionPoint;

		public void Clear()
		{
			_reflectionSpan = null;
		}

		public DateTime DateTime
		{
			get
			{
				DateTime result;
				if (!_stopClock)
				{
					result = DateTime.UtcNow;
					if (_reflectionSpan != null)
					{
						_reflectionSpan -= (DateTime.UtcNow - _ignitionPoint);
						if (_reflectionSpan.Value.Ticks > 0)
						{
							result = result.AddTicks(-_reflectionSpan.Value.Ticks);
						}
						else
						{
							_reflectionSpan = null;
						}
					}
				}
				else
				{
					// The clock isn't actually stopped just ticking once upon each request ie: very slowly as required.
					_ignitionPoint = _ignitionPoint.AddTicks(1);
					result = _ignitionPoint;
				}
				return result;
			}
		}

		private Boolean _stopClock;
	}
}
