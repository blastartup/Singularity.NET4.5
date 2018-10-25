using System;

namespace Singularity.DataService.SqlFramework
{
	public abstract class SqlUnitOfWork<TSqlEntityContext> : IDisposable
		where TSqlEntityContext : SqlEntityContext, new()
	{

		public Boolean Save(Boolean clearContext = false)
		{
			Boolean result = Context.Commit();
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

		public TSqlEntityContext Context => _context ?? (_context = NewDbContext());
		private TSqlEntityContext _context;

		protected virtual TSqlEntityContext NewDbContext()
		{
			return new TSqlEntityContext();	
		}

		protected virtual TSqlEntityContext ResetDbContext()
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
					_disposed = true;
				}
			}
		}

		protected abstract void ResetRepositories();
		private Boolean _disposed = false;
	}
}
