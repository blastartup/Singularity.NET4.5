using System;
using System.Diagnostics;
using System.Globalization;

namespace Singularity
{
	[DebuggerStepThrough]
	public static class CultureInfoExtension
	{
		public static String ResourceForShortNoTitleCaseWords(this CultureInfo currentCulture)
		{
			return ShortNoTitleCaseWordsFormat.FormatX(currentCulture.ThreeLetterISOLanguageName);
		}

		private static readonly String ShortNoTitleCaseWordsFormat = "Singularity.Resources.{0}_ShortNoTitleCaseWords.dic";
	}
}
