using System;
using System.Data;
using System.Data.SqlClient;

namespace Singularity.DataService
{
	public class FillTable : ICommand
	{
		public FillTable(SqlCommand command)
		{
			this._command = command;
		}
	
		public IReply  Execute()
		{
			ReplyDataTable reply = new ReplyDataTable {Value = new DataTable()};

			try
			{
				_command.Connection.Open();
				using (SqlDataAdapter adapter = new SqlDataAdapter(_command))
				{
					adapter.Fill(reply.Value);
				}
			}
			catch (Exception ex)
			{
				reply.ErrorMessage = ex.ToLogString();
			}
			finally
			{
				if (_command.Connection.State == ConnectionState.Open)
				{
					_command.Connection.Close();
				}
			}
			reply.Condition = reply.ErrorMessage.IsEmpty();
			return reply;
		}

		readonly SqlCommand _command;
	}
}
