
namespace Singularity
{
	public class Stray : ICommand
	{
		public Stray(StrayStrategy strategy)
		{
			this._strategy = strategy;
		}

		public IReply Execute()
		{
			return _strategy.Execute();
		}

		private readonly StrayStrategy _strategy;
	}
}
