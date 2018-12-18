using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Singularity.DataService
{
	public interface ISqlGeneratable
	{
		IEnumerable<String> GenerateInsertSql(Object sqlEntity);
		//IEnumerable<String> GenerateUpdateSql(Object sqlEntity);
		//IEnumerable<String> GenerateDeleteSql(Object sqlEntity);
	}
}
