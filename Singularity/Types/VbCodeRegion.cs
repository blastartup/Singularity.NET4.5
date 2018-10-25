using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Types
{
	public class VbCodeRegion : CodeRegion
	{
		protected VbCodeRegion(String regionName, Words sourceCode) : base(regionName, sourceCode)
		{
			InnerStartLineIndex = InnerLines.IndexOf(s => s.Contains("#Region \"{0}\"".FormatX(regionName), StringComparison.OrdinalIgnoreCase));
			if (InnerStartLineIndex > 0)
			{
				InnerEndLineIndex = InnerLines.IndexOf(s => s.Contains("#EndRegion", StringComparison.OrdinalIgnoreCase), InnerStartLineIndex + 1);

				if (InnerEndLineIndex > InnerStartLineIndex)
				{
					InnerLines = InnerLines.GetWords(InnerStartLineIndex + 1, LineIndexCount - 1);
					InnerIsEmpty = InnerLines.Count == 0;
				}
			}
		}

	}
}
