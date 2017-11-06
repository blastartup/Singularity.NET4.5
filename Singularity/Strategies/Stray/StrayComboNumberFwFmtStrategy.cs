using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// Bi-number Fixed Width Stray Strategy.
	/// </summary>
	/// <remarks>Bi number means each half of the total length of digits returned is randomly generated.  This increases
	/// the chance of a pattern of digits in the number, making the number easier to remember and convey.
	/// 
	/// Fixed width ensures the number always starts with a digit of 1 to 9, and returns the full length required.  The length must
	/// be an even number no less than 4 and no greater than 30.
	/// </remarks>
	public class StrayComboNumberFwFmtStrategy : StrayComboNumberFwStrategy
	{
		public StrayComboNumberFwFmtStrategy(Int32 length): base(length)
		{
		}

		public override IReply Execute()
		{
			ReplyMessage result = new ReplyMessage
			{
				Message = ((ReplyInteger)base.Execute()).Value.ToString(),
				Condition = true,
			};
			return result;
		}

		private String Format()
		{
			String result = String.Empty;
			switch (Length)
			{
				case 4:
					result = "0000";
					break;

				case 6:
					result = "000000";
					break;

				case 8:
					result = "0000-0000";
					break;

				case 10:
					result = "00000-00000";
					break;

				case 12:
					result = "000000-000000";
					break;

				case 14:
					result = "000000-000000-00";
					break;

				case 16:
					result = "0000-0000-0000-0000";
					break;

				case 18:
					result = "000000-0000-0000-0000";
					break;

				case 20:
					result = "00000-00000-00000-0000";
					break;
			}
			return result;
		}
	}
}
