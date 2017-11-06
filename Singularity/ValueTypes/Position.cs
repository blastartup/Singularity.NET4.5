using System;

namespace Singularity
{
	//=======================================================================
	/* Project: Geocode.Service
	*  Author: Steve Loughran
	*  Copyright (C) 2000 Steve Loughran
	*  See license.txt or license.html for license and copyright information 
	*  RCS $Header: /cvsroot/geocode/geocode.net/src/library/Position.cs,v 1.4 2000/11/29 08:06:19 steve_l Exp $
	*  jedit:mode=csharp:tabSize=4:indentSize=4:syntax=true:
	*/
	//=======================================================================


	/// <summary>
	///  The representation of a point in polar co-ordinates.
	///	A point consists of a latitude a longitude and a height, and so its
	///	exact location depends upon the datum/TRF in use.
	///	unless otherwise stated, WGS84 is taken as the default datum
	///	height is usually relative to the ellipsoid; although sometimes they
	///	can be sea level relative -it really depends upon the source of altitude
	///	information
	/// </summary>

	[Serializable]

	public class Position : IComparable
	{


		/// <summary>
		/// empty constructor
		/// </summary>

		public Position()
		{
			Clear();
		}


		/// <summary>
		/// detailed constructor
		/// </summary>
		public Position(Double latitude, Double longitude)
		{

			this.Latitude = latitude;
			this.Longitude = longitude;
		}

		/// <summary>
		/// copy constructor
		/// </summary>

		public Position(Position p2)
		{
			this.Latitude = p2.Latitude;
			this.Longitude = p2.Longitude;
		}

		private Double _latitude;
		private Double _longitude;
		private String _name;

		/// <summary>
		///  latitude in degrees. valid range is -90 to +90
		/// (assignments will fix the range up properly)
		/// </summary>


		public Double Latitude
		{
			get
			{
				return _latitude;
			}

			set
			{
				_latitude = value;
				if (_latitude > 90)
					_latitude = 90;
				if (_latitude < -90)
					_latitude = -90;
			}
		}

		/// <summary>
		///  Longitude in Degrees. Valid range -180 to +180
		///  function turns a -180 into +180, it being the same thing, after all.
		/// </summary>

		public Double Longitude
		{
			get
			{
				return _longitude;
			}

			//  fix points into the +-180 range.
			set
			{
				_longitude = value;
				while (_longitude <= -180)
					_longitude += 360;
				while (_longitude > 180)
					_longitude -= 360;
			}
		}

		/// <summary>
		///Name property
		/// </summary>
		public String Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		///  erase the contents
		/// </summary>

		public void Clear()
		{
			_latitude = _longitude = 0;
			_name = String.Empty;
		}

		/// <summary>
		///  zeroness test. 
		/// </summary>
		private Boolean IsZero()
		{ return (_latitude == 0) && (_longitude == 0); }

		/// <summary>
		///  equality test: works on lat and long
		/// </summary>

		public override Boolean Equals(Object o)
		{
			Position pos = (Position)o;
			return _latitude == pos._latitude && _longitude == pos._longitude;
		}


		///  <summary>
		///  set both points
		///  </summary>

		public void SetPosition(Double latitude, Double longitude)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
		}

		/// <summary>
		///  stringify
		/// </summary>

		public override String ToString()
		{
			return String.Format("{0} ({1},{2})", Name, Latitude, Longitude);
		}

		/// <summary>
		/// test for item being in distance. Assumes planet is flat
		///  (that is a valid assumption for small distances, the usual 
		///  cos(theta)==theta for small values of theta assumption that so simplifies
		///  things like pendulum calculations
		/// </summary>

		public Double GetHorizontalRadialDistance(Position p2)
		{
			Double diff = p2.Longitude - Longitude;
			return diff;
		}

		/// <summary>
		///  get distance between latitudes
		/// </summary>

		public Double GetVerticalRadialDistance(Position p2)
		{
			Double diff = p2.Latitude - Latitude;
			return diff;
		}

		/// <summary>
		/// get distance in squared degrees. 
		/// Cant cope with poles or the -180 meridian well. And it assumes
		/// that a degree of longitude is as significant as one of latitude
		/// (which is not the case near the poles).
		/// actually this routine doesn't work that well at all.    
		///  </summary>
		/// <returns> square of the angular distance between points</returns>

		public Double GetSquaredRadialDistance(Position p2)
		{
			Double dx, dy;
			dx = GetHorizontalRadialDistance(p2);
			dy = GetVerticalRadialDistance(p2);
			return (dx * dx) + (dy * dy);
		}

		/// <summary>
		/// hashcode 
		/// </summary>
		/// <returns> name derived hash </returns>

		public override Int32 GetHashCode()
		{
			return _name.GetHashCode();
		}

		/// <summary>
		///  comparison is by location only.
		/// the sort routine is latitude then longitude 
		/// (so northern scotland is &lt;england; england &lt;canada)
		/// </summary>

		Int32 IComparable.CompareTo(Object o)
		{
			Position p = (Position)o;
			if (this.Latitude > p.Latitude)
				return 1;
			if (this.Latitude < p.Latitude)
				return -1;
			//here latitudes are equal; compare on longitude
			if (this.Longitude > p.Longitude)
				return 1;
			if (this.Longitude < p.Longitude)
				return -1;
			return 0;
		}

	} //class 


};
