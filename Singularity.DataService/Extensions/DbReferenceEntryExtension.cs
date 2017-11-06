using System;
using System.Data.Entity.Infrastructure;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class DbReferenceEntryExtension
	{
		public static void LoadAsRequired<TEntity, TElement>(this DbReferenceEntry<TEntity, TElement> collection) 
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
