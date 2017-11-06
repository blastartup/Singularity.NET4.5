using System;
using System.Data;
using System.IO;

namespace Singularity.DataService
{
	public class CsvWriter
	{
		readonly Boolean _isFirstRowHeadings;
		readonly String _seperator;
		
		public CsvWriter(Boolean isFirstRowHeadings = false, String seperator = ",")
		{
			_isFirstRowHeadings = isFirstRowHeadings;
			_seperator = seperator;
		}

		private void OutputToStreamWriter(DataTable dataSource, StreamWriter sw)
		{
			Int32 columnCount = dataSource.Columns.Count;
			Int32 lastColumn = GetLastColumn(columnCount - 1);

			if (_isFirstRowHeadings)
			{
				for (Int32 idx = 0; idx < columnCount; idx++)
				{
					if (!IsRequiredColumn(idx)) continue;
					sw.Write(dataSource.Columns[idx]);
					if (idx < lastColumn)
					{
						sw.Write(_seperator);
					}
				}
				sw.Write(sw.NewLine);
			}

			foreach (DataRow dataRow in dataSource.Rows)
			{
				for (Int32 idx = 0; idx < columnCount; idx++)
				{
					if (!IsRequiredColumn(idx)) continue;
					if (!Convert.IsDBNull(dataRow[idx]))
					{
						sw.Write(dataRow[idx].ToStringSafe().RemoveNoise().Replace(_seperator, ValueLib.Space.StringValue).
						                      Replace(ValueLib.DoubleQuotes.StringValue, String.Empty));
					}

					if (idx < lastColumn)
					{
						sw.Write(_seperator);
					}
				}
				sw.Write(sw.NewLine);
			}
		}

		public Stream ToStream(DataTable dataSource)
		{
			Stream res = new MemoryStream();
			WriteToStream(dataSource, res);
			return res;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="stream"></param>
		public Boolean WriteToStream(DataTable dataSource, Stream stream)
		{
			StreamWriter sw = new StreamWriter(stream);
			OutputToStreamWriter(dataSource, sw);
			sw.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			return true;
		}

		public Boolean WriteToFile(DataTable dataSource, String fileName)
		{
			using (FileStream fs = new FileStream(fileName, FileMode.Create))
			{
				return WriteToStream(dataSource, fs);
			}
		}

		protected virtual Int32 GetLastColumn(Int32 defaultValue)
		{
			return defaultValue;
		}

		protected virtual Boolean IsRequiredColumn(Int32 idx)
		{
			return true;
		}
	}
}
