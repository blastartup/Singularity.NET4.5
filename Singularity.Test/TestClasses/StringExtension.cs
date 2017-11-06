using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.UnitTest
{
	/// <summary>
	/// Summary description for StringExtension
	/// </summary>
	[TestClass]
	public class StringExtension
	{
		public StringExtension()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void TestWord()
		{
			String sentence = "A fat lazy brown queen frog sat on a hairy log.";
			Assert.IsTrue(sentence.Word(1, 2) == "A fat", "First two words");
			Assert.IsTrue(sentence.Word(0, 2) == "A fat", "First two words regardless of < 1 first pos");
		}

		[TestMethod]
      public void TestReplaceFirst()
      {
			String haystack = "$123.00";

			Assert.IsTrue("".ReplaceFirst("sdlfkjs") == "", "Replacing nothing with something returns nothing.");

			Assert.IsTrue(haystack.ReplaceFirst("$") == "123.00", "Replacing a char gets removed.");

			haystack = "$123.00";
			Assert.IsTrue(haystack.ReplaceFirst("$", "%") == "%123.00", "Replacing a char with another gets replaced.");

			haystack = null;
			Assert.IsTrue(haystack.ReplaceFirst("$", "%") == "", "Replacing a null get returns nothing.");

			haystack = "$123.00";
			Assert.IsTrue(haystack.ReplaceFirst("") == "$123.00", "Replacing a nothing, nothing happens.");
			Assert.IsTrue(haystack.ReplaceFirst("", "") == "$123.00", "Replacing a nothing, nothing happens.");
			Assert.IsTrue(haystack.ReplaceFirst("x", "") == "$123.00", "Replacing a non-existent char with nothing, nothing is removed.");
			Assert.IsTrue(haystack.ReplaceFirst("3", "") == "$12.00", "Replacing a char with nothing, char is removed.");
			Assert.IsTrue(haystack.ReplaceFirst(null, "") == "$123.00", "Replacing a null, nothing happens.");
			Assert.IsTrue(haystack.ReplaceFirst("0", null) == "$123.0", "Replacing a char to null, removes char.");
		}

		[TestMethod]
		public void TestToDateTime()
		{
			Assert.IsTrue("2013-02-18 22:04:05".ToDateTime() == new DateTime(2013, 2, 18, 22, 4, 5), "Valid date time.");
			Assert.IsTrue("18-02-2013 22:04:05".ToDateTime() == new DateTime(2013, 2, 18, 22, 4, 5), "Valid date time.");
		}
	}
}
