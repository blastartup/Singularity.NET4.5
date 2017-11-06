/*





		

			CURRENTLY NOT IN USE.








*/

using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService.UniqueCodeService
{
	/* Service x = new Service(new Strategy() { Set Properties }); 
	 * if (!x.Execute())
	 * {
	 *		Something went wrong so loop thru x.Notifications to find out.
	 * }
	 */

	[Obsolete("NOT IN USE.")]
	public abstract class CodeStrategy : ICommand
	{
		public CodeStrategy()
		{
			//alphabetLength = Alphabet.Length - 1;
		}

		#region ICodeStrategy Members

		//public virtual String GenerateCode()
		//{
		//   int start = CurrentCount();
		//   TargetCount = (TargetCount * Convert.ToInt32(!Additional)) + ((TargetCount + start) * Convert.ToInt32(Additional));
		//   for (int i = start; i < TargetCount; i++)
		//   {
		//      string code = GetFirstCode();
		//      while (!IsCodeUnique(code))
		//      {
		//         code = GetSubsequentCode();
		//      }
		//      SaveUniqueCode(code);
		//   }
		//   return code;
		//}

		//public int OwnerID { get; set; }
		//public int CompleteCodeLength { get; set; }
		//public int TargetCount { get; set; }
		//public string BatchCode { get; set; }
		//public bool Additional { get; set; }
		//public INotification Notification { get; set; }

		#endregion

		//public virtual int CurrentCount()
		//{
		//   return CodeServiceStrategy.GetCurrentCount();
		//}

		//public virtual void SetUniqueCode(String code)
		//{
		//   if (CodeServiceStrategy.SaveUniqueCodeAction != null)
		//   {
		//      CodeServiceStrategy.SaveUniqueCodeAction(code);
		//   }
		//}

		#region ICodeServiceStrategy Members

		//CurrentCountQuery ICodeServiceStrategy.GetCurrentCount { get; set; }
		//IsCodeUniqueQuery ICodeServiceStrategy.GetIsCodeUnique { get; set; }
		//Action<string> ICodeServiceStrategy.SaveUniqueCodeAction { get; set; }

		#endregion

		public delegate Int32 CurrentCountQuery();
		public delegate Boolean IsCodeUniqueQuery(String code);

		//ICodeServiceStrategy CodeServiceStrategy
		//{
		//   get { return codeServiceStrategy ?? (codeServiceStrategy = this); }
		//}
		//ICodeServiceStrategy codeServiceStrategy;

		RandomProvider FastRandom
		{
			get { return fastRandom ?? (fastRandom = new RandomProvider()); }
		}
		RandomProvider fastRandom;

		const String alphabet = "ACDEFGHJKMNPQRTUVWXY34679";
		readonly protected Int32 alphabetLength;

		public IReply Execute()
		{
			throw new NotImplementedException();
		}
	}
}
