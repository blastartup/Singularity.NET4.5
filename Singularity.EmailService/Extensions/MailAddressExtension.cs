using System;
using System.Diagnostics;
using System.Net.Mail;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	[DebuggerStepThrough]
	public static class MailAddressExtension
	{
		public static Boolean IsValid(this MailAddress mailAddress)
		{
			return Regex.IsMatch(mailAddress.Address, @"^([\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+\.)*[\w\!\#$\%\&\'\*\+\-\/\=\?\^\`{\|\}\~]+@((((([a-z0-9]{1}[a-z0-9\-]{0,62}[a-z0-9]{1})|[a-z])\.)+[a-z]{2,6})|(\d{1,3}\.){3}\d{1,3}(\:\d{1,5})?)$", RegexOptions.Singleline);
		}

	}
}
