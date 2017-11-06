using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class EnumUtilTest
	{
		[TestMethod]
		public void TestGetEnumAdditionals_SetEnumValue()
		{
			EnumAdditionalAttribute enumAttribute = EnumUtil.GetEnumAdditionals(typeof(TestEnum)).Skip(1).First();
			Assert.IsTrue(enumAttribute.EnumValue == 10, "Second enum value = 10");
		}

		enum TestEnum
		{
			[EnumAdditional("Name", "Code", "Description")]
			FirstEnum,
			[EnumAdditional("Name2", "Code2", "Description2")]
			SecondEnum = 10
		}
	}
}
