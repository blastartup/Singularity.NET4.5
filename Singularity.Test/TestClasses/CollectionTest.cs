using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class CollectionTest
	{
		[TestMethod]
		public void TestDelimitedStringBuilder()
		{
			DelimitedStringBuilder exceptions = new DelimitedStringBuilder();
			Assert.IsTrue(exceptions.IsEmpty(), "No string built.");

			exceptions.Add("blah");
			Assert.IsFalse(exceptions.IsEmpty(), "String built.");
		}
	}
}
