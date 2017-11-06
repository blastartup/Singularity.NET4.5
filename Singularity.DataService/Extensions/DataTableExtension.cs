using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public static class DataTableExtension
	{
		public static DataTable ToDataTable(this ArrayList alist)
		{
			DataTable dt = new DataTable();
			if (alist[0] == null)
				throw new FormatException("Parameter ArrayList empty");
			dt.TableName = alist[0].GetType().Name;
			DataRow dr;
			System.Reflection.PropertyInfo[] propInfo = alist[0].GetType().GetProperties();
			for (Int32 i = 0; i < propInfo.Length; i++)
			{
				dt.Columns.Add(propInfo[i].Name, propInfo[i].PropertyType);
			}
			for (Int32 row = 0; row < alist.Count; row++)
			{
				dr = dt.NewRow();
				for (Int32 i = 0; i < propInfo.Length; i++)
				{
					Object tempObject = alist[row];
					Object t = propInfo[i].GetValue(tempObject, null);
					/*object t =tempObject.GetType().InvokeMember(propInfo[i].Name,    
					 * R.BindingFlags.GetProperty , null,tempObject , new object [] {});*/
					if (t != null)
						dr[i] = t.ToString();
				}
				dt.Rows.Add(dr);
			}
			return dt;
		}

		public static DataTable ToDataTable(this ArrayList alist, ArrayList alColNames)
		{
			DataTable dt = new DataTable();
			if (alist[0] == null)
				throw new FormatException("Parameter ArrayList empty");
			dt.TableName = alist[0].GetType().Name;
			DataRow dr;
			System.Reflection.PropertyInfo[] propInfo = alist[0].GetType().GetProperties();
			for (Int32 i = 0; i < propInfo.Length; i++)
			{
				for (Int32 j = 0; j < alColNames.Count; j++)
				{
					if (alColNames[j].ToString() == propInfo[i].Name)
					{
						dt.Columns.Add(propInfo[i].Name, propInfo[i].PropertyType);
						break;
					}
				}
			}
			for (Int32 row = 0; row < alist.Count; row++)
			{
				dr = dt.NewRow();
				for (Int32 i = 0; i < dt.Columns.Count; i++)
				{
					Object tempObject = alist[row];
					Object t = propInfo[i].GetValue(tempObject, null);
					/*object t =tempObject.GetType().InvokeMember(propInfo[i].Name,   
					 * R.BindingFlags.GetProperty , null,tempObject , new object [] {});*/
					if (t != null)
						dr[i] = t.ToString();
				}
				dt.Rows.Add(dr);
			}
			return dt;
		}

		/// <summary>
		/// Converts a LINQ resultset to a DataTable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="varlist"></param>
		/// <returns></returns>
		public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
		{
			DataTable dtReturn = new DataTable();

			// column names 
			PropertyInfo[] oProps = null;

			if (varlist == null) return dtReturn;

			foreach (T rec in varlist)
			{
				// Use reflection to get property names, to create table, Only first time, others will follow 
				if (oProps == null)
				{
					oProps = ((Type)rec.GetType()).GetProperties();
					foreach (PropertyInfo pi in oProps)
					{
						Type colType = pi.PropertyType;

						if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
						== typeof(Nullable<>)))
						{
							colType = colType.GetGenericArguments()[0];
						}

						dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
					}
				}

				DataRow dr = dtReturn.NewRow();

				foreach (PropertyInfo pi in oProps)
				{
					dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
					(rec, null);
				}

				dtReturn.Rows.Add(dr);
			}
			return dtReturn;
		}

	}
}
