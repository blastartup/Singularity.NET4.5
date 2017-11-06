using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class EntityICollectionExtension
	{
		/// <summary>
		/// Only return Active Singularity Entities
		/// </summary>
		/// <typeparam name="TEntity">An IActivatable object context entity.</typeparam>
		/// <param name="collection">An IQueryable list of IActivatables.</param>
		/// <returns>An IQueryable list of only those entities that are Active.</returns>
		public static IEnumerable<TEntity> Actives<TEntity>(this ICollection<TEntity> collection) where TEntity : class, IDeletable
		{
			return collection.Where(o => o.DeletedDate == null);
		}
	}
}
