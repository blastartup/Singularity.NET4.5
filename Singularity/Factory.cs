using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity
{
	public static class Factory
	{
		public static CultureInfo CurrentCultureInfo
		{
			get { return _current ?? (_current = CultureInfo.CurrentCulture); }
			set { _current = value; }
		}
		[ThreadStatic]
		private static CultureInfo _current;

		public static IDateTimeProvider CurrentDateTimeProvider { get; set; }


		/// <summary>
		/// Instantiate the list with a default starting capacity of 10.
		/// </summary>
		/// <remarks>
		/// By instantiating a list with the approximate capacity to the ultimate count, saves performance hits each time
		/// the capacity needs to be expanded to account for addition of each new item.
		/// </remarks>
		public static List<T> NewList<T>()
		{
			return NewList<T>(EListCapacityType.Minimum);
		}

		/// <summary>
		/// Instantiate the list overriding the Microsoft default starting capacity of 4 by a given integer.
		/// </summary>
		/// <param name="capacity">The starting capcity for the list.</param>
		/// <remarks>
		/// By instantiating a list with the approximate capacity to the ultimate count, saves performance hits each time
		/// the capacity needs to be expanded to account for the large size.  Use (Int32)EListCapacityTypes, a
		/// logrithmic scale of 10, 100, 1000 (Minium, Medium, Maximum respectively) to guessitmate the normal capacity.
		/// </remarks>
		public static List<T> NewList<T>(Int32 capacity)
		{
			if (capacity > (Int32)EListCapacityType.Medium)
			{
				capacity = (Int32)EListCapacityType.Maximum;
			}
			else if (capacity > (Int32)EListCapacityType.Minimum)
			{
				capacity = (Int32)EListCapacityType.Medium;
			}
			else
			{
				capacity = (Int32)EListCapacityType.Minimum;
			}
			return new List<T>(capacity);
		}

		/// <summary>
		/// Instantiate the list overriding the Microsoft default starting capacity of 4 by a EListCapacityTypes enum.
		/// </summary>
		/// <param name="capacityChunk">The starting capcity for the list.</param>
		/// <remarks>
		/// By instantiating a list with the approximate capacity to the ultimate count, saves performance hits each time
		/// the capacity needs to be expanded to account for the large size.  The EListCapacityTypes is a
		/// logrithmic scale of 10, 100, 1000 (Minium, Medium, Maximum respectively).
		/// </remarks>
		public static List<T> NewList<T>(EListCapacityType capacityChunk)
		{
			return new List<T>((Int32)capacityChunk);
		}

		/// <summary>
		/// Instantiate the list with a given collection.
		/// </summary>
		/// <param name="collection">A collection of T elements.</param>
		public static List<T> NewList<T>(IEnumerable<T> collection)
		{
			Contract.Requires(collection != null);

			List<T> internalList = new List<T>((Int32)EListCapacityType.Minimum);
			internalList.AddRange(collection);
			return internalList;
		}

		/// <summary>
		/// Instantiate the list with a given collection.
		/// </summary>
		/// <param name="collection">A collection of T elements.</param>
		public static List<T> NewList<T>(ICollection<T> collection)
		{
			Contract.Requires(collection != null);

			List<T> internalList = new List<T>(collection.Count);
			internalList.AddRange(collection);
			return internalList;
		}


		/// <summary>
		/// Instantiate the list with a given array.
		/// </summary>
		/// <param name="collection">An array of T elements.</param>
		public static List<T> NewList<T>(T[] collection)
		{
			Contract.Requires(collection != null);

			List<T> internalList = new List<T>(collection.Length);
			internalList.AddRange(collection);
			return internalList;
		}

		/// <summary>
		/// Instantiate the list with a given capacity and given enumerable.
		/// </summary>
		/// <param name="capacityChunk">The starting capcity for the list.</param>
		/// <param name="collection">An array of T elements.</param>
		public static List<T> NewList<T>(EListCapacityType capacityChunk, IEnumerable<T> collection)
		{
			Contract.Requires(collection != null);

			List<T> internalList = new List<T>((Int32)capacityChunk);
			internalList.AddRange(collection);
			return internalList;
		}


	}
}
