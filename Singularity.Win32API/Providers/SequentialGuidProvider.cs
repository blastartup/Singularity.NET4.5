using System;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	public static class SequentialGuidProvider
	{
		public static Guid NewGuid()
		{
			if (!_currentGuid.IsEmpty())
			{
				NextGuid();
			}
			else
			{
				_currentGuid = NativeMethods.CreateSequentialGuid();
			}
			return _currentGuid;
		}

		private static Int32[] SqlOrderMap
		{
			get { return _sqlOrderMap ?? (_sqlOrderMap = new [] {3, 2, 1, 0, 5, 4, 7, 6, 9, 8, 15, 14, 13, 12, 11, 10}); }
		}

		private static void NextGuid()
		{
			Byte[] bytes = _currentGuid.ToByteArray();
			for (Int32 mapIndex = 0; mapIndex < 16; mapIndex++)
			{
				Int32 bytesIndex = SqlOrderMap[mapIndex];
				bytes[bytesIndex]++;
				if (bytes[bytesIndex] != 0)
				{
					break; // No need to increment more significant bytes
				}
			}
			_currentGuid = new Guid(bytes);
		}

		private static Int32[] _sqlOrderMap;
		private static Guid _currentGuid;
	}
}
