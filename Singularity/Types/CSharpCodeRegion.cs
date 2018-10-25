using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Types
{
	public class CSharpCodeRegion : CodeRegion
	{
		protected CSharpCodeRegion(String regionName, Words sourceCode) : base(regionName, sourceCode)
		{
			InnerStartLineIndex = InnerLines.IndexOf(s => s.Contains("#region {0}".FormatX(regionName), StringComparison.OrdinalIgnoreCase));
			if (InnerStartLineIndex > 0)
			{
				InnerEndLineIndex = InnerLines.IndexOf(s => s.Contains("#endregion", StringComparison.OrdinalIgnoreCase), InnerStartLineIndex + 1);

				if (InnerEndLineIndex > InnerStartLineIndex)
				{
					InnerLines = InnerLines.GetWords(InnerStartLineIndex + 1, LineIndexCount - 1);
					InnerIsEmpty = InnerLines.Count == 0;
				}
			}
		}

	}
}
