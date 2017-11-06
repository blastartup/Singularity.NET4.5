using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Singularity.DataService
{
	public class QueryExtractraction 
	{
		public QueryExtractraction(String connectionName, String sqlQuery, String folder, String filename)
		{
			_connectionName = connectionName;
			_sqlQuery = sqlQuery;
			_filename = filename;
			_folder = folder;
		}

		public void Execute()
		{
			using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionName].ToString()))
			using (SqlCommand sqlCommand = new SqlCommand(_sqlQuery, connection))
			{
				sqlCommand.CommandTimeout = 240;
				FillTable fillTable = new FillTable(sqlCommand);
				ReplyDataTable outcome = (ReplyDataTable)fillTable.Execute();
				if (outcome.Condition)
				{
					String localFileName = "{0} {1}.csv".FormatX(_filename, DateTime.Now.ToString("yyyy MM dd (dddd)"));
					FileInfo extractFile = new FileInfo(Path.Combine(_folder, localFileName));
					WriteToTextFile exportToFile = new WriteToTextFile(outcome.Value, extractFile, true);
					exportToFile.Execute();
				}
			}
		}

		private readonly String _connectionName;
		private readonly String _sqlQuery;
		private readonly String _folder;
		private readonly String _filename;
	}
}
