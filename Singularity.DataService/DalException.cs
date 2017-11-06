using System;

namespace Singularity.DataService
{
	public class DalException : SystemException
	{
		public DalException(String message, SystemException innerException) : base(message, innerException)
		{
		}
	}
}
