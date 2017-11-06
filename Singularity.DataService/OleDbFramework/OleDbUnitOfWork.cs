using System;

namespace Singularity.DataService.OleDbFramework
{
	public abstract class OleDbUnitOfWork<TOleDbEntityContext> : IDisposable
		where TOleDbEntityContext : OleDbEntityContext, new()
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

		public TOleDbEntityContext Context => _context ?? (_context = NewDbContext());
		private TOleDbEntityContext _context;

		protected virtual TOleDbEntityContext NewDbContext()
		{
			return new TOleDbEntityContext();	
		}

		protected virtual TOleDbEntityContext ResetDbContext()
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
