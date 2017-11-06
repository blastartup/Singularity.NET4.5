
// ReSharper disable once CheckNamespace

namespace Singularity
{
	public static class RoundingProvider
	{
		public static IRoundingProvider Instance
		{
			get
			{
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		private static IRoundingProvider _instance;
	}
}
