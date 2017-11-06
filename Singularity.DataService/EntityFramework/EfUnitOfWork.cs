using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Singularity.DataService
{
	public abstract class EfUnitOfWork<TDbContext> : IDisposable
		where TDbContext : DbContext, new()
	{
		protected EfUnitOfWork()
		{
			_efValidationResults = new List<EfValidationResult>();
		}

		public Boolean Save(Boolean clearContext = false)
		{
			Boolean result;
			try
			{
				result = Context.SaveChanges() > 0;
			}
			catch (DbUpdateException ex)
			{
				result = false;
				_efValidationResults.Clear();
				if (!(ex.InnerException is UpdateException) || !(ex.InnerException.InnerException is SqlException))
				{
					_efValidationResults.Add(new EfValidationResult(ex.Message));
				}
				else
				{
					SqlException sqlException = (SqlException)ex.InnerException.InnerException;
					foreach (SqlError sqlExceptionError in sqlException.Errors)
					{
						Int32 errorNumber = sqlExceptionError.Number;
						String errorText;
						if (_sqlErrorTextDict.TryGetValue(errorNumber, out errorText))
						{
							errorText = $"{errorText} ({errorNumber})";
						}
						else
						{
							errorText = $"Unknown SQL error. ({errorNumber}).";
						}

						_efValidationResults.Add(new EfValidationResult(errorText, ex.Entries.Select(f => f.Entity.GetType().Name)));
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				result = false;
				_efValidationResults.Clear();
				foreach (DbEntityValidationResult validationErrors in ex.EntityValidationErrors)
				{
					foreach (DbValidationError validationError in validationErrors.ValidationErrors)
					{
						_efValidationResults.Add(new EfValidationResult(validationError.ErrorMessage, validationErrors.Entry.Entity.GetType().Name, validationError.PropertyName));
					}
				}
			}

			if (clearContext)
			{
				Context.Dispose();
				_context = ResetDbContext();
				ResetRepositories();
			}
			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public Boolean LazyLoadingEnabled
		{
			get { return Context.Configuration.LazyLoadingEnabled; }
			set { Context.Configuration.LazyLoadingEnabled = value; }
		}

		public TDbContext Context => _context ?? (_context = NewDbContext());
		private TDbContext _context;

		protected virtual TDbContext NewDbContext()
		{
			return new TDbContext();	
		}

		protected virtual TDbContext ResetDbContext()
		{
			return NewDbContext();
		}

		protected virtual void Dispose(Boolean disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					Context.Dispose();
				}
			}
			_disposed = true;
		}

		private static readonly Dictionary<Int32, String> _sqlErrorTextDict =
			 new Dictionary<Int32, String>
		{
		 {547, "This operation failed because another data entry uses this entry."},
		 {2601, "One of the properties is marked as Unique index and there is already an entry with that value."}
		};

		public List<EfValidationResult> EfValidationResults => _efValidationResults;
		private readonly List<EfValidationResult> _efValidationResults;

		protected abstract void ResetRepositories();
		private Boolean _disposed = false;
	}
}
