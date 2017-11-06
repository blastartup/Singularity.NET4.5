using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public static class ICollectionExtension
	{
		/// <summary>
		/// Add a new collection to this existing collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceCollection"></param>
		/// <param name="addCollection"></param>
		public static void AddRange<T>(this ICollection<T> sourceCollection, ICollection<T> addCollection)
		{
			foreach (T item in addCollection)
			{
				sourceCollection.Add(item);
			}
		}

		/// <summary>
		/// Remove a collection from this collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceCollection">Source collection of T type objects.</param>
		/// <param name="removeCollection">Collection of T type objects to remove from the source collection.</param>
		public static void Remove<T>(this ICollection<T> sourceCollection, ICollection<T> removeCollection)
		{
			foreach (T item in removeCollection)
			{
				sourceCollection.Remove(item);
			}
		}

	}
}
