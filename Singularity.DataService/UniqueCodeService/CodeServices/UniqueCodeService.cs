using System;
using System.Data.Entity;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService.UniqueCodeService
{
	public abstract class UniqueCodeService<T> : ICommand where T : DbContext, new() 
	{
		public UniqueCodeService(StrayStrategy strategy)
		{
			this.strategy = strategy;
		}

		public IReply Execute()
		{
			ReplyMessage result = null;
			Int32 cntr = 0;
			do
			{
				result = IsValid(Stray.Execute());
				cntr++;
			} while(!result.Condition && cntr < 4);
			return result;
		}

		protected virtual ReplyMessage IsValid(IReply generatedCodeResult)
		{
			ReplyMessage result = (ReplyMessage)generatedCodeResult;
			result.Condition = IsCodeUnique(result.Message) && (result.Message.Length == strategy.CompleteLength);
			return result;
		}

		//protected void SetCodeServiceStrategy()
		//{
		//   ICodeServiceStrategy codeServiceStrategy = (ICodeServiceStrategy)Strategy;
		//   //codeServiceStrategy.GetCurrentCount = new CodeStrategy.CurrentCountQuery(ExecuteCurrentCountQuery);
		//   codeServiceStrategy.GetIsCodeUnique = new CodeStrategy.IsCodeUniqueQuery(ExecuteIsCodeUniqueQuery);
		//   codeServiceStrategy.SaveUniqueCodeAction = new Action<String>(SaveUniqueCode);

		//   SetCodeServiceStrategyCore();
		//}

		//protected virtual void SetCodeServiceStrategyCore()
		//{
		//   // Override this to set db and functionality specific strategies...  (see TCCCrm.Data.UniqueCodeService)
		//}

		public Action PreExecutionAction { get; set; }
		public Action PostExecutionAction { get; set; }

		//protected virtual int ExecuteCurrentCountQuery()
		//{
		//   return 0;
		//}

		protected virtual Boolean IsCodeUnique(String code)
		{
			return true;
		}

		protected Stray Stray
		{
			get { return stray ?? (stray = new Stray(strategy)); }
		}
		Stray stray;

		protected T DB
		{
			get { return db ?? (db = new T()); }
		}
		T db;

		readonly StrayStrategy strategy;
	}
}
