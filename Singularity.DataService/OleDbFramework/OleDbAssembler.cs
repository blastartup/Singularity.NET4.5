using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace Singularity.DataService.OleDbFramework
{
	public abstract class OleDbAssembler
	{
		protected OleDbAssembler(OleDbDataReader oleDbDataReader)
		{
			DataReader = oleDbDataReader;
		}

		/// <summary>
		/// Safely return a value of a given column from a SqlDataReader, whether or not that column name was specified in the original SQL command.
		/// </summary>
		/// <typeparam name="TValue">The data type expected of the column value returned.</typeparam>
		/// <param name="columnName">Name of column whose value is to be returned.</param>
		/// <returns>Should the column exist, then its value is returned.  If the value is DBNull or the column doesn't exist, the default value of the T data type is returned.</returns>
		/// <remarks>Currently Nothing (AKA null) is never returned.</remarks>
		protected TValue ReadValue<TValue>(String columnName)
		{
			return ReadValue(columnName, default(TValue));
		}

		protected TValue ReadValue<TValue>(String columnName, TValue defaultValue)
		{
			TValue result = defaultValue;
			try
			{
				// Massive performance improvement here...
				if (ColumnNameExists(columnName))
				{
					Object value = DataReader[columnName];
					if (value != null && value != DBNull.Value)
					{
						result = (TValue)value;
					}
				}
			}
			catch(IndexOutOfRangeException) { }
			catch(InvalidCastException) { }
			

			return result;
		}

		private Boolean ColumnNameExists(String columnName)
		{
			if (_columnNames == null)
			{
				_columnNames = new List<String>(DataReader.FieldCount);
				if (DataReader.HasRows && DataReader.FieldCount > 0)
				{
					for (Int32 idx = 0; idx < DataReader.FieldCount; idx++)
					{
						_columnNames.Add(DataReader.GetName(idx));
					}
				}
			}
			return _columnNames.Any(f => f.Equals(columnName, StringComparison.OrdinalIgnoreCase));
		}
		private List<String> _columnNames;

		public OleDbDataReader DataReader { get; private set; }
	}

	public abstract class OleDbAssembler<TClass> : OleDbAssembler
		where TClass : class, new()
	{
		protected OleDbAssembler(OleDbDataReader oleDbDataReader) : base(oleDbDataReader)
		{
		}

		public List<TClass> AssembleClassList()
		{
			List<TClass> classList = new List<TClass>();
			if (DataReader == null)
			{
				return classList;
			}

			while (DataReader.Read())
			{
				TClass newClass = new TClass();
				AssembleClassCore(newClass);
				classList.Add(newClass);
			}
			return classList;
		}

		/// <summary>
		/// After performing a Read(), assemble the Class if a row exists.
		/// </summary>
		/// <returns>InvoiceItemClass populated by your database read.</returns>
		/// <remarks>Bypasses the default behaviour where the AssembleClass will automatically do a Read() for you.</remarks>
		public TClass ReadAndAssembleClass()
		{
			TClass newClass = null;
			if (DataReader == null)
			{
				return newClass;
			}

			if (DataReader.Read())
			{
				newClass = new TClass();
				AssembleClassCore(newClass);
			}
			return newClass;
		}

		/// <summary>
		/// Assemble the Class after you have already called a Read() prior to calling this Function and a row exists.
		/// </summary>
		/// <returns>InvoiceItemClass populated by your database read.</returns>
		/// <remarks>Bypasses the default behaviour where the AssembleClass will automatically do a Read() for you.</remarks>
		public TClass AssembleClass()
		{
			TClass newClass = null;
			if (DataReader.HasRows)
			{
				newClass = new TClass();
				AssembleClassCore(newClass);
			}
			return newClass;
		}

		protected abstract void AssembleClassCore(TClass newClass);
	}
}
