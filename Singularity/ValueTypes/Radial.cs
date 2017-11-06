using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerStepThrough]
	public struct Radial : IStateEmpty
	{
		public Radial(Double? latitude, Double? longitude, Int32 radius)
			: this(new Coord(latitude, longitude), radius)
		{
		}

		public Radial(Double latitude, Double longitude, Int32 radius) : this(new Coord(latitude, longitude), radius)
		{
		}

		public Radial(Coord coord, Int32 radius)
		{
			this._coord = coord;
			this._radius = radius;
		}

		public Coord Coord
		{
			get { return _coord; }
			set { _coord = value; }
		}

		private Coord _coord;

		public Int32 Radius
		{
			get { return _radius; }
			set { _radius = value; }
		}

		private Int32 _radius;

		public Boolean IsEmpty
		{
			get { return _coord.IsEmpty || _radius.IsEmpty(); }
		}
	}
}