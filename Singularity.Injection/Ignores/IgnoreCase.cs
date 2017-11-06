using System.Reflection;
using System;

namespace Singularity.Injection.Ignores
{
	public class IgnoreCase : BaseIgnoreInjection
	{
		public IgnoreCase() { }

		public IgnoreCase(String[] ignoredProps)
			: base(ignoredProps)
		{
		}

		protected override PropertyInfo GetTargetProperty(Type targetType, String spName)
		{
			return targetType.GetProperty(spName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);
		}
	}
}
