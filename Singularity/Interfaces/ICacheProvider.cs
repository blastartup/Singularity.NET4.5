using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public interface ICacheProvider<TKey, TValue>
	{
		/// <summary>
		/// Retrieve cached item or after adding a new item to the cache.
		/// </summary>
		TValue GetOrAdd(TKey key, TValue newValue);

		/// <summary>
		/// Retrieve cached item or new uncached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Name of cached item</param>
		/// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
		/// <returns>Cached item as type</returns>
		Boolean GetOrNew(TKey key, out TValue value);

		/// <summary>
		/// Insert value into the cache using
		/// appropriate name/value pairs
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="value">Item to be cached</param>
		/// <param name="key">Name of item</param>
		void Set(TKey key, TValue value);

		/// <summary>
		/// Remove item from cache
		/// </summary>
		/// <param name="key">Name of cached item</param>        
		void Clear(TKey key);

		IEnumerable<KeyValuePair<TKey, TValue>> GetAll();
	}

	public interface ICacheTimedProvider<TKey, TValue> : ICacheProvider<TKey, TValue>
	{
		/// <summary>
		/// Insert value into the cache using
		/// appropriate name/value pairs WITH a cache duration set in minutes
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Item to be cached</param>
		/// <param name="value">Name of item</param>
		/// <param name="duration">Cache duration in minutes</param>
		void Set(TKey key, TValue value, Int32 duration);
	}
}
