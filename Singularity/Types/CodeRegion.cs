using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity;

namespace Singularity
{
	public class CodeRegion : IStateEmpty
	{
		public CodeRegion(String regionName, WordCollection sourceCode)
		{
			_isEmpty = true;
			_name = regionName;
			_lines = sourceCode;
			_startLineIndex = _lines.IndexOf(s => s.Contains("#region {0}".FormatX(_name)));
			if (_startLineIndex > 0)
			{
				_endLineIndex = _lines.IndexOf(s => s.Contains("#endregion"), _startLineIndex + 1);

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

		public String Name
		{
			get { return _name; }
		}
		private String _name;

		public Int32 StartLineIndex
		{
			get { return _startLineIndex; }
		}
		private Int32 _startLineIndex;

		public Int32 EndLineIndex
		{
			get { return _endLineIndex; }
		}
		private Int32 _endLineIndex;

		public Int32 LineIndexCount
		{
			get { return _endLineIndex - _startLineIndex; }
		}

		public WordCollection Lines
		{
			get { return _lines; }
		}
		private WordCollection _lines;

		public Boolean IsEmpty 
		{
			get { return _isEmpty; }
		}
		private Boolean _isEmpty;
	}
}
