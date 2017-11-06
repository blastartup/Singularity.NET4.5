﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Singularity.DataService.SqlFramework
{
	public class SqlEntityContext : IDisposable
	{
		public SqlEntityContext(SqlConnectionStringBuilder sqlConnectionStringBuilder)
		{
			_sqlConnectionStringBuilder = sqlConnectionStringBuilder;
			_sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
			_sqlConnection.Open();
			_transactionCounter = 0;
		}

		public SqlDataReader ExecDataReader(String query, SqlParameter[] filterParameters)
		{
			using (SqlCommand cmd = CreateCommand(query, CommandType.Text, filterParameters))
			{
				return ExecuteWithRetry(cmd, cmd.ExecuteReader);
			}
		}

		public Object ExecScalar(String query, SqlParameter[] filterParameters)
		{
			using (SqlCommand cmd = CreateCommand(query, CommandType.Text, filterParameters))
			{
				return ExecuteWithRetry(cmd, cmd.ExecuteScalar);
			}
		}

		public Int32 ExecuteNonQuery(String query, SqlParameter[] filterParameters)
		{
			using (SqlCommand cmd = CreateCommand(query, CommandType.Text, filterParameters))
			{
				return ExecuteWithRetry(cmd, cmd.ExecuteNonQuery);
			}
		}

		public SqlTransaction BeginTransaction()
		{
			return _sqlTransaction ?? (_sqlTransaction = _sqlConnection.BeginTransaction());
		}

		public Boolean Commit()
		{
			Boolean result = false;
			if (_sqlTransaction != null)
			{
				try
				{
					_sqlTransaction.Commit();
					result = true;
				}
				catch (InvalidOperationException)
				{
					_sqlTransaction.Rollback();
				}
				finally
				{
					_sqlTransaction.Dispose();
					_sqlTransaction = null;
				}
			}
			return result;
		}

		public void Rollback()
		{
			if (_sqlTransaction != null)
			{
				_sqlTransaction.Rollback();
				_sqlTransaction.Dispose();
				_sqlTransaction = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(Boolean disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_sqlTransaction?.Dispose();
					if (_sqlConnection != null)
					{
						_sqlConnection.Close();
						_sqlConnection.Dispose();
					}

					_disposed = true;
				}
			}
		}

		private SqlCommand CreateCommand(String query, CommandType commandType, SqlParameter[] filterParameters)
		{
			SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection)
			{
				CommandType = commandType,
				Transaction = _sqlTransaction
			};
			sqlCommand.Parameters.AddRange(filterParameters);
			return sqlCommand;
		}

		private T ExecuteWithRetry<T>(SqlCommand cmd, Func<T> executeSql)
		{
			_errorMessage = String.Empty;
			Boolean reconnect = false;
			for (Int32 idx = 0; idx < MaximumRetries; idx++)
			{
				try
				{
					if (reconnect)
					{
						if (_sqlConnection != null)
						{
							_sqlConnection.Close();
							_sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
							_sqlConnection.Open();
							cmd.Connection = _sqlConnection;
							_transactionCounter = 0;
						}
					}
					else if (_sqlConnection.State == ConnectionState.Closed)
					{
						if (_sqlConnection.ConnectionString.IsEmpty())
						{
							_sqlConnection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
						}
						_sqlConnection.Open();
						_transactionCounter = 0;
					}
					_transactionCounter++;
					return executeSql();
				}
				catch (InvalidOperationException)
				{
					reconnect = true;
				}
				catch (SqlException ex)
				{
					if (ex.Errors.Cast<SqlError>().All(CanRetry))
					{
						Thread.Sleep(DelayOnError);
						reconnect = ex.Errors.Cast<SqlError>().Any(f => f.Class >= 20);
						continue;
					}
					break;
				}
			}

			return default(T);
		}

		private Boolean CanRetry(SqlError error)
		{
			if (error.Class == 16)
			{
				// Invalid object name 'InvalidObjectName'.
				_errorMessage = error.Message;
				return false;
			}
			return true;
		}

		public Boolean AutomaticTransactions { get; set; }
		public String Name => _sqlConnectionStringBuilder.InitialCatalog;
		public String ErrorMessage => _errorMessage;

		public SqlConnection SqlConnection => _sqlConnection;
		private SqlConnection _sqlConnection;

		private const Int32 MaximumRetries = 3;
		private const Int32 DelayOnError = 500;
		private Int32 _transactionCounter;
		private const Int32 MaximumTransactionsBeforeReconnect = 10000;
		private SqlTransaction _sqlTransaction;
		private Boolean _disposed;
		private String _errorMessage;
		private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

	}
}
