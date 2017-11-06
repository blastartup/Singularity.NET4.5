using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[TypeConverter(typeof(BaseNTypeConverter))]
	[DebuggerDisplay("{MValue}")]
	public struct BaseN : IComparable, IComparable<BaseN>, IComparable<Int32>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Must be an int, short, byte, FShort, FByte or FBaseN.</param>
		//[DebuggerStepThrough]
		public BaseN(Object value)
		{
			if (value is Int32)
			{
				this = new BaseN((Int32)value);
			}
			else
			{
				BaseNTypeConverter lTypeConverter = new BaseNTypeConverter();
				if (value == null || lTypeConverter.CanConvertFrom(value.GetType()))
				{
					this = (BaseN)lTypeConverter.ConvertFrom(value);
				}
				else
				{
					throw new FormatException("The given argument (aValue) type ({0}) is not supported by {1}.".FormatX(value.GetType(), typeof(BaseN)));
				}
			}
			this._mBaseN = 10;
		}

		//[DebuggerStepThrough]
		public BaseN(Int32 aValue)
		{
			_mBaseN = 10;
			MValue = aValue;
			_mDefault = 0;
			_mCeiling = 0;
			_mFloor = 0;
		}

		public Byte Base
		{
			get { return _mBaseN; }
			set
			{
				Contract.Requires(value >= MinBase && value <= MaxBase);
				_mBaseN = value;
			}
		}

		private Byte _mBaseN;

		public static readonly Int32 MinBase = 2;
		public static readonly Int32 MaxCodeBase = 29;
		public static readonly Int32 MaxBase = 36;
		public static readonly String BaseAlphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		/// <summary>
		/// Is this int within a given range?
		/// </summary>
		public Boolean IsInRange(BaseN aLowValue, BaseN aHighValue)
		{
			return (this >= aLowValue && this <= aHighValue);
		}

		public override Boolean Equals(Object aObject)
		{
			return (aObject is BaseN && (BaseN)aObject == this) || (aObject is Int32 && (Int32)aObject == MValue);
		}

		public override Int32 GetHashCode()
		{
			return MValue.GetHashCode();
		}

		public override String ToString()
		{
			return ToStringCore(_mBaseN);
		}

		public String ToString(Byte aBase)
		{
			return ToStringCore(aBase);
		}

		private String ToStringCore(Byte aBase)
		{
			String lResult = String.Empty;
			if (aBase == 10)
			{
				lResult = MValue.ToString();
			}
			else
			{
				Int32 lCalcValue = Math.Abs(MValue);
				Int32 lDigitNumber;
				do
				{
					lDigitNumber = lCalcValue - (aBase * (Int32)(lCalcValue / aBase));
					lResult = lResult.Insert(0, BaseAlphabet[lDigitNumber]);
					lCalcValue = (lCalcValue - lDigitNumber) / aBase;
				}
				while (lCalcValue > 0);
				lResult = lResult.Insert(0, Sign);
			}
			return lResult;
		}

		#region Casting

		[DebuggerStepThrough]
		public static implicit operator BaseN(Int32 aValue)
		{
			return new BaseN(aValue);
		}

		[DebuggerStepThrough]
		public static implicit operator BaseN(Byte aValue)
		{
			return new BaseN(Convert.ToInt32(aValue));
		}

		[DebuggerStepThrough]
		public static implicit operator BaseN(Int16 aValue)
		{
			return new BaseN(Convert.ToInt32(aValue));
		}

		[DebuggerStepThrough]
		public static implicit operator Int32(BaseN aValue)
		{
			return aValue.MValue;
		}

		[DebuggerStepThrough]
		public static explicit operator Byte(BaseN aValue)
		{
			return Convert.ToByte(aValue.MValue);
		}

		[DebuggerStepThrough]
		public static explicit operator Int16(BaseN aValue)
		{
			return Convert.ToInt16(aValue.MValue);
		}

		[DebuggerStepThrough]
		public static explicit operator Decimal(BaseN aValue)
		{
			return new Decimal(aValue);
		}

		#endregion

		#region Operator Overloads

		[DebuggerStepThrough]
		public static Boolean operator ==(BaseN leftUnit, BaseN rightleftDecimalUnitnit)
		{
			return leftUnit.MValue.Equals(rightleftDecimalUnitnit.MValue);
		}

		[DebuggerStepThrough]
		public static Boolean operator !=(BaseN leftUnit, BaseN rightDoubleUnit)
		{
			return !(leftUnit.MValue.Equals(rightDoubleUnit.MValue));
		}

		public static BaseN operator +(BaseN aValue, BaseN aAddition)
		{
			return new BaseN(aValue.MValue + aAddition.MValue) { Base = aValue.Base };
		}

		public static BaseN operator +(BaseN aValue, Int32 aAddition)
		{
			return new BaseN(aValue.MValue + aAddition) { Base = aValue.Base };
		}

		public static BaseN operator -(BaseN aValue, BaseN aSubtraction)
		{
			return new BaseN(aValue.MValue - aSubtraction.MValue) { Base = aValue.Base };
		}

		public static BaseN operator -(BaseN aValue, Int32 aSubtraction)
		{
			return new BaseN(aValue.MValue - aSubtraction) { Base = aValue.Base };
		}

		public static BaseN operator ++(BaseN aValue)
		{
			return new BaseN(aValue.MValue + 1) { Base = aValue.Base };
		}

		public static BaseN operator --(BaseN aValue)
		{
			BaseN lResult = new BaseN(aValue.MValue - 1)
			{
				Base = aValue.Base
			};
			return lResult;
		}

		#endregion

		#region Parsing
		/// <summary>
		/// Can the given string be converted to a FBaseN?
		/// </summary>
		/// <param name="aValue">A string containing a base 10 number to convert.</param>
		public static Boolean CanParse(String aValue)
		{
			BaseN lTempFBaseN;
			return TryParse(aValue, out lTempFBaseN);
		}

		/// <summary>
		/// The integer equivalent of the given string.
		/// </summary>
		/// <param name="aValue">A string containing a number to convert.</param>
		public static BaseN Parse(String aValue)
		{
			BaseN lResult;
			return TryParse(aValue, out lResult) ? lResult : Zero;
		}

		/// <summary>
		/// Can the given string be converted to an integer? If so, returns the converted integer, else zero.
		/// </summary>
		/// <param name="aValue">A string containing a number to convert.</param>
		/// <param name="aResult">Returned as the integer equivalent of the given string, else zero if it cannot be converted.</param>
		public static Boolean TryParse(String aValue, out BaseN aResult)
		{
			Double lTryParseResult;
			Boolean lSuccess = Double.TryParse(aValue, NumberStyles.Integer, Factory.CurrentCultureInfo.NumberFormat, out lTryParseResult)
				&& CanConvertToFBaseN(lTryParseResult);
			aResult = lSuccess ? (BaseN)lTryParseResult : Zero;
			return lSuccess;
		}

		private static Boolean CanConvertToFBaseN(Double aValue)
		{
			return (aValue >= Int32.MinValue && aValue <= Int32.MaxValue);
		}
		#endregion

		#region IFType Members

		public Boolean IsEmpty
		{
			get { return MValue == 0; }
		}

		public Boolean IsValid
		{
			get { return true; }
		}

		public Boolean IsDefault
		{
			get { return this == _mDefault; }
		}

		public BaseN Default
		{
			get { return new BaseN(_mDefault); }
			set { _mDefault = value.MValue; }
		}

		private Int32 _mDefault;

		public BaseN Ceiling
		{
			get { return new BaseN(_mCeiling); }
			set { _mCeiling = value.MValue; }
		}

		private Int32 _mCeiling;

		public BaseN Floor
		{
			get { return new BaseN(_mFloor); }
			set { _mFloor = value.MValue; }
		}

		private Int32 _mFloor;

		#endregion

		#region IComparable and Generic Members

		public Int32 CompareTo(Object aOther)
		{
			return MValue.CompareTo(aOther);
		}

		public Int32 CompareTo(BaseN aOther)
		{
			return MValue.CompareTo(aOther.MValue);
		}

		public Int32 CompareTo(Int32 aOther)
		{
			return MValue.CompareTo(aOther);
		}

		#endregion

		private String Sign
		{
			get { return (MValue < 0) ? ValueLib.MinusSign.StringValue : String.Empty; }
		}

		public static readonly BaseN Zero = new BaseN(0);
		internal readonly Int32 MValue;
	}
}

//#region Testing
//#if DEBUG
//namespace IsadoreSoftware.Foundation.Testing
//{
//   using NUnit.Framework;

//   [TestFixture]
//   public class FBaseNTest
//   {
//      [Test]
//      public void TestInvalidBase()
//      {
//         FBaseN lValue = new FBaseN(13);
//         Assert.AreEqual(true, lValue.IsValid, "Is valid");

//         Assert.DoesNotThrow(() => lValue.BaseN = 2, "Changing to base 2 is a valid operation");
//      }

//      [Test]
//      public void TestNullValue()
//      {
//         FBaseN lBaseN;
//         Assert.DoesNotThrow(() => lBaseN = new FBaseN(null), "Passing a null value is handled correctly");

//         lBaseN = new FBaseN(null);
//         Assert.AreEqual(0, (int)lBaseN, "Default value set");
//      }

//      [Test]
//      public void TestBase10()
//      {
//         FBaseN lValue = new FBaseN(13);
//         Assert.AreEqual(10, lValue.BaseN, "Base 10 is the default base");
//         Assert.AreEqual("13", lValue.ToString(), "Same value");

//         lValue++;
//         Assert.AreEqual("14", lValue.ToString(), "Addition");

//         FBaseN lValue2 = new FBaseN(2);
//         lValue += lValue2;
//         Assert.AreEqual("16", lValue.ToString(), "Addition with FBaseN");

//         lValue -= lValue2;
//         Assert.AreEqual("14", lValue.ToString(), "Subtraction with FBaseN");

//         lValue += 1;
//         Assert.AreEqual("15", lValue.ToString(), "Addition with int");

//         lValue -= 10;
//         Assert.AreEqual("5", lValue.ToString(), "Subtraction with int");

//         lValue--;
//         Assert.AreEqual("4", lValue.ToString(), "Subtraction");
//      }

//      [Test]
//      public void TestBase2()
//      {
//         FBaseN lValue = new FBaseN(13);
//         lValue.BaseN = 2;
//         Assert.AreEqual("1101", lValue.ToString(), "Binary value");

//         lValue++;
//         Assert.AreEqual("1110", lValue.ToString(), "Addition");

//         lValue -= 20;
//         Assert.AreEqual("-110", lValue.ToString(), "Negative binary");
//         Assert.AreEqual("-20", lValue.ToString(3), "Negative trinary");
//      }

//   }
//}
//#endif
//#endregion
