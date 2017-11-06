using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;

namespace Singularity.Google
{
	public class GoogleProvider
	{
		public OAuth2Parameters OAuth2Parameters(String clientId, String clientSecret)
		{
			return new OAuth2Parameters()
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				Scope = scope,
				RedirectUri = "urn:ietf:wg:oauth:2.0:oob", // ??
			};
		}

		private String scope
		{
			get { return "https://spreadsheets.google.com/feeds https://docs.google.com/feeds"; }
		}

	}
}
