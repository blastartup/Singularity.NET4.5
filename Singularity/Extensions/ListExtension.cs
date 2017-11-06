using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class ListExtension
	{
		/// <summary>
		/// Extend the FList to only add an element that is not empty.
		/// </summary>
		/// <param name="element">Any object or type that can be empty.</param>
		public static void AddNonEmpty<T>(this List<T> list, T element)
		{
			if (!element.IsEmpty())
			{
				list.Add(element);
			}
		}

		/// <summary>
		/// Extend the FList to only add elements that are not empty.
		/// </summary>
		/// <param name="elements">A collection of elememts of which only the non empty ones will be added.</param>
		public static void AddRangeNonEmpty<T>(this List<T> list, IEnumerable<T> elements)
		{
			foreach (T lElement in elements)
			{
				AddNonEmpty(list, lElement);
			}
		}

		/// <summary>
		/// Converts the elements in the current <see cref="T:System.Collections.Generic.List`1"/> to another type, and returns a list containing the converted elements.
		/// </summary>
		/// <typeparam name="TInput">Generic data type of source list.</typeparam>
		/// <param name="list">List to convert from.</param>
		/// 
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.List`1"/> of the target type containing the converted elements from the current <see cref="T:System.Collections.Generic.List`1"/>.
		/// </returns>
		/// <param name="converter">A <see cref="T:System.Converter`2"/> delegate that converts each element from one type to another type.</param>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam><exception cref="T:System.ArgumentNullException"><paramref name="converter"/> is null.</exception>
		public static List<TOutput> ConvertAll<TInput, TOutput>(this List<TInput> list, Converter<TInput, TOutput> converter) where TOutput : IComparable<TOutput>
		{
			Contract.Requires(converter != null);

			List<TOutput> result = new List<TOutput>(list.Count);
			result.AddRange(list.ConvertAll(converter));
			return result;
		}

		/// <summary>
		/// Get an element at index and remove it from the list too.
		/// </summary>
		/// <typeparam name="T">Generic data type of list.</typeparam>
		/// <param name="list">List to remove item from.</param>
		/// <param name="index">The index of the item to remove.</param>
		/// <returns>The value of the item at the given index.</returns>
		public static T Extract<T>(this List<T> list, Int32 index)
		{
			if (index > list.Count)
			{
				return default(T);
			}

			T result = list[index];
			list.RemoveAt(index);
			return result;
		}

		/// <summary>
		/// Retrieves all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// 
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.List`1"/> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:System.Collections.Generic.List`1"/>.
		/// </returns>
		/// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the elements to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="match"/> is null.</exception>
		public static List<T> FindAll<T>(this List<T> list, Predicate<T> match)
		{
			Contract.Requires(match != null);

			List<T> result = new List<T>(list.Count);
			result.AddRange(list.FindAll(match));
			result.TrimExcess();
			return result;
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the 
		/// zero-based index of the first occurrence within the range of elements in the list.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by 
		/// <paramref name="match"/>, if found; otherwise, –1.
		/// </returns>
		/// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the element to search for.</param><exception cref="T:System.ArgumentNullException"><paramref name="match"/> is null.</exception>
		public static Int32 FindIndex<T>(this List<T> list, Predicate<T> match)
		{
			Contract.Requires(match != null);

			return list.FindIndex(0, 1, match);
		}

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate, and returns the 
		/// zero-based index of the first occurrence within the range of elements in the list that starts at the specified
		/// index.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <returns>
		/// The zero-based index of the first occurrence of an element that matches the conditions defined by 
		/// <paramref name="match"/>, if found; otherwise, –1.
		/// </returns>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the element 
		/// to search for.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="match"/> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="startIndex"/> is outside 
		/// the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1"/>.</exception>
		public static Int32 FindIndex<T>(this List<T> list, Int32 startIndex, Predicate<T> match)
		{
			Contract.Requires(startIndex >= 0);
			Contract.Requires(match != null);

			return list.FindIndex(startIndex, 1, match);
		}

		/// <summary>
		/// Find last index of an item starting in the list based on a predicated match.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <param name="match">Predication to match on.</param>
		/// <returns></returns>
		public static Int32 FindLastIndex<T>(this List<T> list, Predicate<T> match)
		{
			Contract.Requires(match != null);

			return list.FindLastIndex(0, 1, match);
		}

		/// <summary>
		/// Find last index of an item starting from a given point in the list based on a predicated match.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <param name="startIndex">Index to start from.</param>
		/// <param name="match">Predication to match on.</param>
		/// <returns>An integer indicating the index of the found item.</returns>
		public static Int32 FindLastIndex<T>(this List<T> list, Int32 startIndex, Predicate<T> match)
		{
			Contract.Requires(startIndex > 0);
			Contract.Requires(match != null);

			return list.FindLastIndex(startIndex, 1, match);
		}

		/// <summary>
		/// Get the first count items from the list.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <param name="count">First number of items to get.</param>
		/// <returns>A List of items.</returns>
		public static List<T> GetRange<T>(this List<T> list, Int32 count)
		{
			return list.GetRange(0, Math.Min(list.Count, count));
		}

		/// <summary>
		/// Get a sub list of non-empty items.
		/// </summary>
		/// <typeparam name="T">Generic type of list.</typeparam>
		/// <param name="list">List from which to return a sub set of.</param>
		/// <param name="index">Starting index.</param>
		/// <param name="count">Maximum number of items to return.</param>
		/// <returns>A list of non-empty items.  If the original list is empty, an empty list is returned.</returns>
		public static List<T> GetRangeNonEmpty<T>(this List<T> list, Int32 index, Int32 count)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(count > 0);

			List<T> result = new List<T>(list.Count);
			if (list.Count > 0)
			{
				index = Math.Min(list.Count - 1, index).LimitMin(0);
				Int32 limit = Math.Min(list.Count - index, count).LimitMin(1);

				result.AddRangeNonEmpty(list.GetRange(index, limit));
				result.TrimExcess();
			}
			return result;
		}

		/// <summary>
		/// Get the index of the nth occurrence of an item as predicated starting from a particular index.
		/// </summary>
		/// <typeparam name="T">Generic data type of list.</typeparam>
		/// <param name="list">List to go through.</param>
		/// <param name="predicate">Conditions to find.</param>
		/// <param name="index">Starting from index.</param>
		/// <param name="occurrence">Number of occurrences of the precication.</param>
		/// <returns></returns>
		public static Int32 IndexOf<T>(this List<T> list, Predicate<T> predicate, Int32 index = 0, Int32 occurrence = 1)
		{
			Contract.Requires(predicate != null);
			Contract.Requires(index >= 0);
			Contract.Requires(occurrence > 0);

			Int32 foundCounter = 0;
			for (Int32 idx = index; idx < list.Count; idx++)
			{
				if (predicate.Invoke(list[idx]))
				{
					if (++foundCounter == occurrence)
					{
						return idx;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Safely insert non-null items from a collection at a particular index.
		/// </summary>
		/// <param name="list">Current list into which to insert a new collection.</param>
		/// <param name="index">Point of insertion.</param>
		/// <param name="collection">An unsafe collection or items to insert.</param>
		public static void InsertRangeSafely<T>(this List<T> list, Int32 index, IEnumerable<T> collection)
		{
			Contract.Requires(collection != null);

			Collection<T> lSafeCollection = new Collection<T>();
			foreach (T lItem in collection)
			{
				if (lItem != null) lSafeCollection.Add(lItem);
			}
			list.Capacity += lSafeCollection.Count;
			list.InsertRange(index, lSafeCollection);
		}



	}
}
