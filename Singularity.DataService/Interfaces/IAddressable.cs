using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public interface IAddressable
	{
		String AddressLine1 { get; set; }
		String AddressLine2 { get; set; }
		String AddressLine3 { get; set; }
		String Suburb { get; set; }
		String State { get; set; }
		String PostalCode { get; set; }
	}
}
