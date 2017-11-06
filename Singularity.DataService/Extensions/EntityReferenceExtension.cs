using System;
using System.Data.Entity.Core.Objects.DataClasses;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class EntityReferenceExtension
	{
		public static void LoadAsRequired(this EntityReference reference)
		{
			if (!reference.IsLoaded)
			{
				reference.Load();
			}
		}

		public static void LoadAsRequired<TEntity>(this EntityReference<TEntity> reference) where TEntity : class
		{
			if (!reference.IsLoaded)
			{
				try
				{
					reference.Load();
				}
				catch (InvalidOperationException) { }
			}
		}
	}
}
