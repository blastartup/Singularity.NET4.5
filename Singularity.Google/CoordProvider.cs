using System;
using GeoCoding;
using GeoCoding.Google;

namespace Singularity.Google
{
	public class Google
	{
		public static Coord GetLatLong(String accessKey, String address1, String address2, String suburb, String state, String postcode, String country)
		{
			Coord coord = new Coord();
			String address = address1;
			if (address2 != String.Empty)
			{
				address += " " + address2;
			}
			IGeoCoder geoCoder = new GoogleGeoCoder(accessKey);
			Address[] addresses = geoCoder.GeoCode(address, suburb, state, postcode, country);
			if (!addresses.IsEmpty())
			{
				coord.Latitude = addresses[0].Coordinates.Latitude;
				coord.Longitude = addresses[0].Coordinates.Longitude;
			}
			return coord;
		}
	}
}
