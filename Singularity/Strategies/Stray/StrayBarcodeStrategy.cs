using System;
using System.Diagnostics;
using System.Text;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class StrayBarcodeStrategy : StrayStrategy
	{
		public StrayBarcodeStrategy(Int32 length, Int16 checkSumLength)
			: base(length)
		{
			if (length < 4 || length > 20)
			{
				throw new InvalidOperationException("Length exceeds boundary requires 4 =< length =< 30.");
			}
			this.CheckSumLength = checkSumLength;
			this.Length = length - checkSumLength;
		}

		[DebuggerStepThrough]
		public override IReply Execute()
		{
			ReplyMessage result = new ReplyMessage();
			StringBuilder numberBuilder = new StringBuilder();
			for (Int32 idx = 0; idx < Length; idx++)
			{
				numberBuilder.Append(ValueLib.BarcodeAlphabet[Random.Next(0, 28)]);
			}
			numberBuilder.Append(numberBuilder.ToString().GetChecksum(CheckSumLength));
			result.Message = numberBuilder.ToString();
			result.Condition = true;
			return result;
		}

		protected readonly Int32 Length;
		protected readonly Int16 CheckSumLength = 1;
	}
}
