using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.FileService
{
	public static class StringExtension
	{
		public static Boolean IsValidFilename(this String fileName)
		{
			return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
		}
	}
}
