using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Singularity;

namespace Singularity.Test
{
	[TestClass]
	public class IComparableExtenstion
	{
		[TestMethod]
		public void TestExtensionElse()
		{
			Int32 y = 1;
			Assert.IsTrue(y.ElseIf(v => v > 0, 10) == 1, "Else condition not triggered.");

			y = 0;
			Assert.IsTrue(y.ElseIf(v => v > 0, 10) == 10, "Else condition was triggered.");
		}
	}
}
