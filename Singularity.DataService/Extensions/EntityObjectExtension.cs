using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class EntityObjectExtension
	{
		public static TEntity Clone<TEntity>(this TEntity entityObject) where TEntity : EntityObject, new()
		{
			TEntity result = new TEntity();

			PropertyInfo[] sourceProperties = entityObject.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public);
			PropertyInfo[] targetProperties = result.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public);
			foreach (PropertyInfo sourceProperty in sourceProperties)
			{
				PropertyInfo targetProperty = targetProperties.First(p => p.Name == sourceProperty.Name);
				targetProperty.SetValue(result, sourceProperty.GetValue(entityObject, null), null);
			}

			return result;
		}
	}
}
