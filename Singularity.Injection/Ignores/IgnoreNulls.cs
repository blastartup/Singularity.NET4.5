using System;
using System.Reflection;
using Omu.ValueInjecter.Injections;

namespace Singularity.Injection.Ignores
{
	public class IgnoreNulls : IgnoreCase
	{
		public IgnoreNulls() { }

		public IgnoreNulls(String[] ignoredProps)
			: base(ignoredProps)
		{
		}

		protected override void SetValue(Object source, Object target, PropertyInfo sp, PropertyInfo tp)
		{
			if (sp.GetValue(source) != null)
			{
				tp.SetValue(target, sp.GetValue(source));
			}
		}
	}
}
