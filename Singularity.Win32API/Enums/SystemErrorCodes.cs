using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	public enum SystemErrorCodes
	{
		[EnumAdditional("Error Session Credential Conflict", "Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed. Disconnect all previous connections to the server or shared resource and try again.")]
		ErrorSessionCredentialConflict = 1219
	}
}
