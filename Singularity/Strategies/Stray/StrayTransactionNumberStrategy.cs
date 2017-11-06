using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public class StrayTransactionNumberStrategy : StrayStrategy
	{
		public StrayTransactionNumberStrategy(Guid debitAccountId) : base(10)
		{
			_debitAccountCheckDigit = VerhoeffCheckDigit.CalculateCheckDigit(debitAccountId.ToStringSafe().RemoveNonNumeric());
		}

		[DebuggerStepThrough]
		public override IReply Execute()
		{
			ReplyMessage reply = new ReplyMessage();

			BaseN monthOfYear = DateTime.UtcNow.Month;
			monthOfYear.Base = 16;

			/* Returns cnNNNNNNCM where;
			 *  c = Check digit for Debit PinMoney account.
			 *  n = A random number between 1 - 9.
			 *  NNNNNN = A random number between 0 - 999999.
			 *  C = A check digit for cnNNNNNC.
			 *  D = A digit representing the day of the week.
			 *  M = A hex digit representing the month of the calendar year.
			 *  
			 * and the whole number is unique. */

			Int32 transactionNumber = _debitAccountCheckDigit * 100000000 + Random.Next(1, 9) * 10000000 + Random.Next(0, 999999) * 10;
			transactionNumber += VerhoeffCheckDigit.CalculateCheckDigit(transactionNumber);
			reply.Message = "{0}{1}".FormatX(transactionNumber, monthOfYear.ToString());
			reply.Condition = true;
			return reply;
		}

		private readonly Int32 _debitAccountCheckDigit;
	}
}
