using System;
using System.Data;
using System.IO;

namespace Singularity.DataService
{
	/// <summary>
	/// 
	/// </summary>
	public class WriteToTextFile : ICommand
	{
		public WriteToTextFile(DataTable dataSource, FileInfo outputFilePath, Boolean isFirstRowHeadings = false, String seperator = ",")
		{
			_dataSource = dataSource;
			_outputFilePath = outputFilePath;
			_writer = new CsvWriter(isFirstRowHeadings,seperator);
		}

		public WriteToTextFile(DataTable dataSource, FileInfo outputFilePath, CsvWriter writer)
		{
			_dataSource = dataSource;
			_outputFilePath = outputFilePath;
			_writer = writer;
		}

		public IReply Execute()
		{
			ReplyDataTable reply = new ReplyDataTable();
			try
			{
				using (FileStream outputFile = new FileStream(_outputFilePath.FullName, FileMode.Create))
				{
					_writer.WriteToStream(_dataSource, outputFile);
					outputFile.Close();
				}
			}
			catch (Exception ex)
			{
				reply.ErrorMessage = ex.ToLogString();
			}
			return reply;
		}

		private readonly DataTable _dataSource;
		private readonly FileInfo _outputFilePath;
		private readonly CsvWriter _writer;
	}
}
