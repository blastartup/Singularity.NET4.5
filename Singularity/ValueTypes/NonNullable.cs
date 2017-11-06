using System;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	///<summary>A simple structure to wrap a class reference after ensuring that the reference is not null.</summary>
	[DebuggerStepThrough]
	public struct NonNullable<T> where T : new()
	{
		/// <summary>Check and wrap a value.</summary>
		/// <param name = "aItem">The value to check and wrap.</param>
		/// <remarks>To pass a non null value use ?? operator.</remarks>
		public NonNullable(T aItem)
		{
			if (aItem == null)
			{
				_mValue = new T();
			}
			else
			{
				_mValue = aItem;
			}
		}

		///<summary>Get a String representation of the wrapped value.</summary>
		///<returns>The result of the wrapped value's ToString().</returns>
		public override String ToString()
		{
			return (Value.ToString());
		}

		///<summary>Implicit wrapping of the value.</summary>
		///<returns>The wrapped value.</returns>
		///<exception cref="System.ArgumentNullException">If Value is a null reference.</exception>
		public static implicit operator NonNullable<T>(T aItem)
		{
			return (new NonNullable<T>(aItem));
		}

		///<summary>Implicit unwrapping of the value.</summary>
		///<returns>The unwrapped value.</returns>
		public static implicit operator T(NonNullable<T> aItem)
		{
			return (aItem.Value);
		}

		/// <summary>The wrapped value.</summary>
		[DebuggerHidden]
		public T Value
		{
			get { return _mValue; }
		}
		private T _mValue;
	}
}

//#region TestCase
//#if DEBUG
//namespace CargoWise.Common.Testing
//{
//   using NUnit.Framework;

//   public class NonNullableTest : TestCase
//   {
//      [ExpectNoExceptions()]
//      public void TestNonNullableWithNoExceptions()
//      {
//         NonNullable<AnyClassForTesting> nonNullAnyClass = new NonNullable<AnyClassForTesting>();
//         AssertNotNull(nonNullAnyClass);
//         AssertNull("NonNullStruct value returns null", nonNullAnyClass.Value);

//         // Alternate syntax.
//         AnyClassForTesting aClassForTesting = null;
//         nonNullAnyClass = new NonNullable<AnyClassForTesting>(aClassForTesting ?? (new AnyClassForTesting()));
//         AssertNotNull(nonNullAnyClass.Value);
//         AssertEquals("AnyClassForTesting has default property value", "DefaultValue", nonNullAnyClass.Value.Property);

//         aClassForTesting = new AnyClassForTesting("InstanceValue");
//         nonNullAnyClass = new NonNullable<AnyClassForTesting>(aClassForTesting ?? (new AnyClassForTesting()));
//         AssertNotNull(nonNullAnyClass);
//         AssertEquals("AnyClassForTesting has instance property value", "InstanceValue", nonNullAnyClass.Value.Property);
//      }

//      [ExpectException(typeof(System.ArgumentNullException))]
//      public void TestNonNullableWithArgumentNullException_OnConstruction()
//      {
//         NonNullable<AnyClassForTesting> nonNullAnyClass = new NonNullable<AnyClassForTesting>(null);
//      }

//      public void TestToString()
//      {
//         AnyClassForTesting aClassForTesting = new AnyClassForTesting();
//         NonNullable<AnyClassForTesting> nonNullAnyClass = new NonNullable<AnyClassForTesting>(aClassForTesting);
//         AssertEquals("Original or default ToString()", "DefaultValue", nonNullAnyClass.ToString());
//         nonNullAnyClass.Value.Property = "NewValue";
//         AssertEquals("Original or default ToString()", "NewValue", nonNullAnyClass.ToString());
//      }

//      [ExpectNoExceptions()]
//      public void TestCastingWithNoExceptions()
//      {
//         AnyClassForTesting aClassForTesting = new AnyClassForTesting();
//         NonNullable<AnyClassForTesting> wrappedValue = aClassForTesting;
//         AssertEquals("NonNullable value hasn't changed.", wrappedValue.Value, aClassForTesting);

//         AnyClassForTesting anotherClassForTesting = wrappedValue;
//         AssertEquals("Cast out class hasn't changed.", anotherClassForTesting, aClassForTesting);

//         AnyClassForTesting cloned = AnyClassForTesting.Clone(wrappedValue);
//      }

//      [ExpectException(typeof(System.ArgumentNullException))]
//      public void TestCastingWithArgumentNullExceptionOnCasting()
//      {
//         AnyClassForTesting aClassForTesting = null;
//         NonNullable<AnyClassForTesting> wrappedValue = aClassForTesting;
//      }

//      class AnyClassForTesting
//      {
//         public AnyClassForTesting()
//         {
//            Property = "DefaultValue";
//         }

//         public AnyClassForTesting(string value)
//         {
//            Property = value;
//         }

//         public static AnyClassForTesting Clone(AnyClassForTesting value)
//         {
//            AnyClassForTesting result = new AnyClassForTesting();
//            result.Property = value.Property;
//            return result;
//         }

//         public string Property { get; set; }

//         public override string ToString()
//         {
//            return Property;
//         }
//      }
//   }
//}
//#endif
//#endregion

