using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity;

namespace Singularity
{
	public abstract class CodeRegion : IStateEmpty
	{
		protected CodeRegion(String regionName, Words sourceCode)
		{
			_isEmpty = true;
			_name = regionName;
			_lines = sourceCode;
			_startLineIndex = _lines.IndexOf(s => s.Contains("#region {0}".FormatX(_name), StringComparison.OrdinalIgnoreCase));
			if (_startLineIndex > 0)
			{
				_endLineIndex = _lines.IndexOf(s => s.Contains("#endregion", StringComparison.OrdinalIgnoreCase), _startLineIndex + 1);

				if (_endLineIndex > _startLineIndex)
				{
					_lines = _lines.GetWords(_startLineIndex + 1, LineIndexCount - 1);
					_isEmpty = _lines.Count == 0;
				}
			}
		}

		public void Sort()
		{
			_lines.Sort();
		}

		public String Name => _name;
		private String _name;

		public Int32 StartLineIndex => _startLineIndex;

		protected Int32 InnerStartLineIndex
		{
			get { return _startLineIndex; }
			set { _startLineIndex = value; }
		}
		private Int32 _startLineIndex;

		public Int32 EndLineIndex => _endLineIndex;

		protected Int32 InnerEndLineIndex
		{
			get { return _endLineIndex; }
			set { _endLineIndex = value; }
		}
		private Int32 _endLineIndex;

		public Int32 LineIndexCount => _endLineIndex - _startLineIndex;

		public Words Lines => _lines;

		protected Words InnerLines
		{
			get { return _lines; }
			set { _lines = value; }
		}
		private Words _lines;

		public Boolean IsEmpty => _isEmpty;

		protected Boolean InnerIsEmpty
		{
			get { return _isEmpty; }
			set { _isEmpty = value; }
		}
		private Boolean _isEmpty;
	}
}
