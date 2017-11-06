using System;

// ReSharper disable once CheckNamespace

namespace Singularity.EmailService
{
	public struct EmailFrom : IMessageFrom
	{
		public EmailFrom(String address)
		{
			Address = address;
		}

		public String Address { get; }

		public override Boolean Equals(Object obj)
		{
			return (obj is EmailFrom && ((EmailFrom)obj).Address == Address);
		}

		public override Int32 GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				Int32 hash = (Int32)2166136261;
				// Suitable nullity checks etc, of course :)
				hash = hash * 16777619 ^ Address.GetHashCode();
				return hash;
			}
		}
	}
}
