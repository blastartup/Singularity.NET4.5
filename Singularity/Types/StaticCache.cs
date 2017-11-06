using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	//[DebuggerStepThrough]
	public class StaticCache<TKey, TValue> : Cacher<TKey, TValue> where TValue : class, new()
	{
		protected override IDictionary<TKey, TValue> InitCache()
		{
			return new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// Retrieve cached item or after adding a new item to the cache.
		/// </summary>
		public override TValue GetOrAdd(TKey key, TValue newValue)
		{
			if (Cache.ContainsKey(key))
			{
				return Cache[key];
			}

			Cache.Add(key, newValue);
			return newValue;
		}

		/// <summary>
		/// Retrieve cached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Name of cached item</param>
		/// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
		/// <returns>Cached item as type</returns>
		public override Boolean GetOrNew(TKey key, out TValue value)
		{
			bool result = false;
			if (Cache.ContainsKey(key))
			{
				value = Cache[key];
				result = true;
			}
			else
			{
				value = default(TValue);
			}

			return result;
		}

		/// <summary>
		/// Insert value into the cache using
		/// appropriate name/value pairs
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="value">Item to be cached</param>
		/// <param name="key">Name of item</param>
		public override void Set(TKey key, TValue value)
		{
			if (Cache.ContainsKey(key))
			{
				Cache[key] = value;
			}
			else
			{
				Cache.Add(key, value);
			}
		}

		public TValue GetOrEvaluate(TKey key, Func<TKey, TValue> value)
		{
			TValue result;
			if (Cache.ContainsKey(key))
			{
				result = Cache[key];
			}
			else
			{
				result = value(key);
				Cache.Add(key, result);
			}
			return result;
		}

		/// <summary>
		/// Remove item from cache
		/// </summary>
		/// <param name="key">Name of cached item</param>        
		public override void Clear(TKey key)
		{
			Cache.Remove(key);
		}

		public override IEnumerable<KeyValuePair<TKey, TValue>> GetAll()
		{
			return Cache.Select(item => new KeyValuePair<TKey, TValue>(item.Key, item.Value));
		}
	}
}
