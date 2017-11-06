using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class DelimitedStringTest
	{
		[TestMethod]
		public void TestIsEmpty()
		{
			DelimitedStringBuilder builder = null;
			Assert.IsTrue(builder.IsEmpty(), "Null instance is empty.");

			builder = new DelimitedStringBuilder();
			Assert.IsTrue(builder.IsEmpty(), "Empty contents is empty.");

			builder.AddIfNotEmpty(null);
			Assert.IsTrue(builder.IsEmpty(), "Adding null is supported but still empty.");

			builder.AddIfNotEmpty("");
			Assert.IsTrue(builder.IsEmpty(), "Adding empty to this method is still empty.");

			builder.AddIfNotEmpty("something");
			Assert.IsTrue(!builder.IsEmpty(), "Finally this is not empty.");
		}

	}
}
