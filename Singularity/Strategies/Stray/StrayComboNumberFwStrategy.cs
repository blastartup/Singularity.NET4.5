using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Bi-number Fixed Width Stray Strategy.
	/// </summary>
	/// <remarks>Combo number means the number returned is made of smaller randomly generated number units.  This increases
	/// the chance of a pattern of digits in the result, making the number easier to remember and convey.
	/// 
	/// Fixed width ensures the number always starts with a digit of 1 to 9, and returns the full length required.  The length must
	/// be an even number no less than 4 and no greater than 20.
	/// </remarks>
	public class StrayComboNumberFwStrategy : StrayStrategy
	{
		public StrayComboNumberFwStrategy(Int32 length) : base(length)
		{
			if (length < 4 || length > 20)
			{
				throw new InvalidOperationException("Length exceeds boundary requires 4 =< length =< 30.");
			}

			if (!length.IsEven())
			{
				throw new InvalidOperationException("Length must be an even number.");
			}

			Length = length;
			Int32 upperExponential = length / 2;
			_upperBound = (Int32)Math.Pow(10, upperExponential);
			Int32 lowerExponential = upperExponential - 1;
			_lowerBound = (Int32)Math.Pow(10, lowerExponential);
		}

		public override IReply Execute()
		{
			ReplyInteger reply = new ReplyInteger
			{
				Value = Random.Next(_lowerBound, _upperBound)*_upperBound + Random.Next(_lowerBound, _upperBound),
				Condition = true
			};
			return reply;
		}

		protected readonly Int32 Length;
		private readonly Int32 _upperBound;
		private readonly Int32 _lowerBound;
	}
}
