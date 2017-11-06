using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singularity.Test
{
	[TestClass]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void TestKeepNumericCharacters()
		{
			const String validChars = "0123456789";

			const String invalidChars = " \r\n !@#$%^&*()_+ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

			TestPermutations(StringExtension.KeepNumericCharacters, validChars, invalidChars);

		}

		[TestMethod]
		public void TestKeepAlphanumericCharacters()
		{
			const String validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789";

			const String invalidChars = "\r\n!@#$%^&*()_+";

			TestPermutations(StringExtension.KeepAlphanumericCharacters, validChars, invalidChars);
		}

		void TestPermutations(Func<String,String> func, String validChars, String invalidChars)
		{

			Assert.AreEqual(validChars, func(validChars)); //as is

			Assert.AreEqual(validChars, func(invalidChars + validChars)); //prefix

			Assert.AreEqual(validChars, func(validChars + invalidChars)); //suffix

			Assert.AreEqual(validChars, func(invalidChars + validChars + invalidChars)); //both ends

			Assert.AreEqual(validChars, func( validChars.Insert(validChars.Length/2,invalidChars))); //middle

			Assert.AreEqual(validChars, func(invalidChars + validChars.Insert(validChars.Length / 2, invalidChars) + invalidChars)); //both ends and middle
		}
	}
}
