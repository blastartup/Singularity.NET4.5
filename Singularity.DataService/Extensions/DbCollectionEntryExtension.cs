using System;
using System.Data.Entity.Infrastructure;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class DbCollectionEntryExtension
	{
		public static void LoadAsRequired<TEntity, TElement>(this DbCollectionEntry<TEntity, TElement> collection) 
			where TEntity : class
		{
			if (!collection.IsLoaded)
			{
				try
				{
					collection.Load();
				}
				catch (InvalidOperationException) { }
			}
		}
	}
}
