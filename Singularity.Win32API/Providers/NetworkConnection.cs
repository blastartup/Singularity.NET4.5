using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	public class NetworkConnection : IDisposable
	{

		public NetworkConnection(String networkName, NetworkCredential credentials, Boolean reuseExisting = false)
		{
			_networkName = networkName;

			NetResource netResource = new NetResource()
			{
				Scope = ResourceScope.GlobalNetwork,
				ResourceType = ResourceType.Disk,
				DisplayType = ResourceDisplaytype.Share,
				RemoteName = networkName
			};

			String userName = String.IsNullOrEmpty(credentials.Domain)
				 ? credentials.UserName
				 : $@"{credentials.Domain}\{credentials.UserName}";

			Int32 result = WNetAddConnection2(netResource, credentials.Password, userName, 0);

			if (result != 0 && !(reuseExisting && result.In((Int32)SystemErrorCodes.ErrorSessionCredentialConflict)))
			{
				throw new Win32Exception(result, "Error connecting to remote share");
			}
		}

		~NetworkConnection()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(Boolean disposing)
		{
			WNetCancelConnection2(_networkName, 0, true);
		}

		[DllImport("mpr.dll")]
		private static extern Int32 WNetAddConnection2(NetResource netResource, String password, String username, Int32 flags);

		[DllImport("mpr.dll")]
		private static extern Int32 WNetCancelConnection2(String name, Int32 flags, Boolean force);

		private readonly String _networkName;
	}
}
