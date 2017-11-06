using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter.Injections;

// ReSharper disable once CheckNamespace

namespace Singularity.Injection.Ignores
{
	public abstract class BaseIgnoreInjection : LoopInjection
	{
		protected BaseIgnoreInjection() { }

		protected BaseIgnoreInjection(String[] ignoredProps)
			: base(ignoredProps)
		{
		}

		protected override void Execute(PropertyInfo sp, Object source, Object target)
		{
			if (!sp.CanRead || ignoredProps != null && Enumerable.Contains(ignoredProps, sp.Name))
				return;
			PropertyInfo property = GetTargetProperty(target.GetType(), GetTargetProp(sp.Name));
			if (!(property != null) || !property.CanWrite || !MatchTypes(sp.PropertyType, property.PropertyType))
				return;
			SetValue(source, target, sp, property);
		}

		protected virtual PropertyInfo GetTargetProperty(Type targetType, String spName)
		{
			return targetType.GetProperty(spName);
		}
	}
}
