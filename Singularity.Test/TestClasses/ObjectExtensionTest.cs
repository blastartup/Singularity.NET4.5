using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class ObjectExtensionTest
	{
		[TestMethod]
		public void TestToStringSafe()
		{
			DateTime? testval = new DateTime(2013,07,31);
			Assert.AreEqual("2013-07-31T00:00:00",testval.ToStringSafe());
		}
	}
}
