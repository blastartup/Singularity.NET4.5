using System;
using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	//[DebuggerStepThrough]
	public abstract class Cacher<TKey, TValue> : ICacheProvider<TKey, TValue> where TValue : class, new()
	{
		protected Cacher()
		{
			Cache = InitCache();
		}

		protected IDictionary<TKey, TValue> Cache;

		protected abstract IDictionary<TKey, TValue> InitCache();

		/// <summary>
		/// Retrieve cached item or after adding a new item to the cache.
		/// </summary>
		public abstract TValue GetOrAdd(TKey key, TValue newValue);

		/// <summary>
		/// Retrieve cached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Name of cached item</param>
		/// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
		/// <returns>Cached item as type</returns>
		public abstract Boolean GetOrNew(TKey key, out TValue value);

		/// <summary>
		/// Insert value into the cache using
		/// appropriate name/value pairs
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="value">Item to be cached</param>
		/// <param name="key">Name of item</param>
		public abstract void Set(TKey key, TValue value);

		/// <summary>
		/// Remove item from cache
		/// </summary>
		/// <param name="key">Name of cached item</param>        
		public abstract void Clear(TKey key);

		public abstract IEnumerable<KeyValuePair<TKey, TValue>> GetAll();

	}
}
