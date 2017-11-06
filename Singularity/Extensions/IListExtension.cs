using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

// ReSharper disable once CheckNamespace
// ReSharper disable once InconsistentNaming

namespace Singularity
{
	/// <summary>
	/// Extensions to IList.
	/// </summary>
	[DebuggerStepThrough]
	public static class IListExtension
	{
		#region Sorting

		/*	Bubble sort		0 to   50
		 * Quick Sort	  51 to  500
		 * Merge Sort	 501 to 1000
		 * Heap Sort   1001+
		*/

		/// <summary>
		/// An extension of IList&gt;T&lt; to sort the given list.
		/// </summary>
		/// <typeparam name="T">The data type of the give list.</typeparam>
		/// <param name="list">The list to sort.</param>
		/// <remarks>
		/// The most efficient sorting algorithym is used depending on the size of the given list.
		/// </remarks>
		public static void Sort<T>(this IList<T> list) where T : IComparable<T>
		{
			Contract.Requires(list != null);
			if (list.Count == 0)
			{
				return;
			}

			if (list.Count <= 50)
			{
				BubbleSort(ref list);
			}
			else if (list.Count <= 500)
			{
				QuickSort(ref list);
			}
			else if (list.Count <= 1000)
			{
				MergeSort(ref list);
			}
			else
			{
				HeapSort(ref list);
			}
		}


		#region "quick sort"

		private static void QuickSort<T>(ref IList<T> list) where T : IComparable<T>
		{
			list = QuickSortCore(list);
		}

		private static IList<T> QuickSortCore<T>(IList<T> list) where T : IComparable<T>
		{
			RandomProvider random = new RandomProvider();

			if (list.Count <= 1)
				return list;

			Int32 pivotIndex = random.Next(list.Count);
			IList<T> lowList = new List<T>(pivotIndex);
			IList<T> highList = new List<T>(list.Count - pivotIndex);

			T pivot = list[pivotIndex];
			list.RemoveAt(pivotIndex);

			foreach (T element in list)
			{
				if (element.CompareTo(pivot) <= 0)
				{
					lowList.Add(element);
				}
				else
				{
					highList.Add(element);
				}
			}

			return ConcatentateLists(list, QuickSortCore(lowList), pivot, QuickSortCore(highList));
		}

		private static IList<T> ConcatentateLists<T>(IList<T> list, IList<T> integersLessThanList, T pivot, 
			IList<T> integersGreaterThanList) where T : IComparable<T>
		{
			IList<T> result = new List<T>(integersLessThanList.Count + integersGreaterThanList.Count + 1);
			result.AddRange(integersLessThanList);
			result.Add(pivot);
			result.AddRange(integersGreaterThanList);
			return result;
		}

		#endregion

		#region "bubble sort"

		private static void BubbleSort<T>(ref IList<T> list) where T : IComparable<T>
		{
			for (Int32 lOuterIdx = 1; lOuterIdx <= list.Count; lOuterIdx++)
			{
				for (Int32 lInnerIdx = 0; lInnerIdx < list.Count - lOuterIdx; lInnerIdx++)
				{
					if (list[lInnerIdx].CompareTo(list[lInnerIdx + 1]) == 1)
					{
						T temp = list[lInnerIdx];
						list[lInnerIdx] = list[lInnerIdx + 1];
						list[lInnerIdx + 1] = temp;
					}
				}
			}
		}

		#endregion

		#region "merge sort"

		private static void MergeSort<T>(ref IList<T> list) where T : IComparable<T>
		{
			MergeSortInternal(ref list);
		}

		private static void MergeSortInternal<T>(ref IList<T> list) where T : IComparable<T>
		{
			IList<T> result = new List<T>(list.Count);
			IList<T> leftList = new List<T>(list.Count / 2);
			IList<T> rightList = new List<T>(list.Count / 2);
			if (list.Count <= 1)
				return;


			Int32 middle = list.Count / 2;
			for (Int32 i = 0; i < middle; i++)
				leftList.Add(list[i]);
			for (Int32 i = middle; i < list.Count; i++)
				rightList.Add(list[i]);

			MergeSortInternal(ref leftList);
			MergeSortInternal(ref rightList);

			if (leftList[leftList.Count - 1].CompareTo(rightList[0]) <= 0)
			{
				leftList.AddRange(rightList);
				list = leftList;
				return;
			}

			list = Merge(leftList, rightList);
		}

		private static IList<T> Merge<T>(IList<T> leftList, IList<T> rightList) where T : IComparable<T>
		{
			List<T> result = new List<T>((Int32)(((Single)(leftList.Count + rightList.Count) * .5f) + 1f));
			while (leftList.Count > 0 && rightList.Count > 0)
			{
				if (leftList[0].CompareTo(rightList[0]) < 0)
				{
					result.Add(leftList[0]);
					leftList.RemoveAt(0);
				}
				else
				{
					result.Add(rightList[0]);
					rightList.RemoveAt(0);
				}
			}

			while (leftList.Count > 0)
			{
				result.Add(leftList[0]);
				leftList.RemoveAt(0);
			}

			while (rightList.Count > 0)
			{
				result.Add(rightList[0]);
				rightList.RemoveAt(0);
			}

			return result;
		}

		#endregion

		#region "Heap Sort"

		private static void HeapSort<T>(ref IList<T> list) where T : IComparable<T>
		{
			Int32 i;
			T temp;

			for (i = (list.Count / 2) - 1; i >= 0; i--)
				SiftDown(list, i, list.Count);

			for (i = list.Count - 1; i >= 1; i--)
			{
				temp = list[0];
				list[0] = list[i];
				list[i] = temp;
				SiftDown(list, 0, i - 1);
			}
		}

		private static void SiftDown<T>(IList<T> list, Int32 root, Int32 bottom) where T : IComparable<T>
		{
			T temp;
			Int32 done, maxChild;

			done = 0;
			while ((root * 2 <= bottom) && (done == 0))
			{
				if (root * 2 == bottom)
					maxChild = root * 2;
				else if (list[root * 2].CompareTo(list[root * 2 + 1]) > 0)
					maxChild = root * 2;
				else
					maxChild = root * 2 + 1;

				if (list[root].CompareTo(list[maxChild]) < 0)
				{
					temp = list[root];
					list[root] = list[maxChild];
					list[maxChild] = temp;
					root = maxChild;
				}
				else
					done = 1;
			}
		}

		#endregion

		#endregion

		public static String ToDelimitedString(this IList<String> list, String delimiter = ",")
		{
			DelimitedStringBuilder delimitedStringBuilder = new DelimitedStringBuilder(list);
			return delimitedStringBuilder.ToDelimitedString(delimiter);
		}
	}
}
