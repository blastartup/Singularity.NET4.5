using System.Linq;

// ReSharper disable InconsistentNaming
// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	/// <summary>
	/// Static class to hold extenstion methods for the IOrderedEnumerable type
	/// </summary>
	public static class IOrderedEnumerableExtension
	{
		public static IOrderedQueryable<TEntity> AsOrderedQueryable<TEntity>(this IOrderedEnumerable<TEntity> collection)
		{
			return collection.AsQueryable().OrderBy(s => s);
		}
	}
}
