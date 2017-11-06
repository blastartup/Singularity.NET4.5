using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class KeyValuePairsTest
	{
		[TestMethod]
		public void TestConstructor()
		{
			String value = String.Empty;
			KeyValuePairs kvp = new KeyValuePairs(value, ValueLib.SemiColon.CharValue, ValueLib.EqualsSign.CharValue);
			Assert.IsNotNull(kvp, "Even though an empty string was passed, make sure it doesn't fail.");
		}
	}
}
