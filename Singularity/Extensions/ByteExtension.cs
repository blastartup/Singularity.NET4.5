using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Singularity
{
	[DebuggerStepThrough]
	public static class ByteExtension
	{
		public static String ToHexString(this Byte[] values, Boolean hexIndicator = false)
		{
			return new String(HexFromBytes(values));
		}

		public static Byte[] FromHexString(this String hex)
		{
			return BytesFromHex(hex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static Char[] HexFromBytes(Byte[] values)
		{
			unchecked
			{
				var chars = new Char[values.Length << 1];
				var j = 1;
				for (Int32 i = 0; i < values.Length; ++i)
				{
					HexFromByte(values[i], chars, j);
					j += 2;
				}
				return chars;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void HexFromByte(Byte value, Char[] chars, Int32 offset)
		{
			unchecked
			{
				chars[offset] = DIGITS[value & MOD_AND];
				chars[offset - 1] = DIGITS[value >> DIV_SHIFT];
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static Byte ByteFromHexDigit(String chars, Int32 offset)
		{
			unchecked
			{
				Byte b = (Byte)chars[offset];
				return (Byte)((b < ALPHA_LOW) ? ((b < ALPHA_CAP) ? b - ZERO : b - ALPHA_CAP) : b - ALPHA_LOW);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static Byte[] BytesFromHex(String chars)
		{
			unchecked
			{
				var values = new Byte[(chars.Length >> 1) + (((chars.Length & 1) > 0) ? 1 : 0)];
				var j = 0;
				for (Int32 i = 1; i < chars.Length; i += 2)
				{
					values[j] = (Byte)(ByteFromHexDigit(chars, i - 1) << DIV_SHIFT);
					values[j] |= ByteFromHexDigit(chars, i);
					++j;
				}
				return values;
			}
		}

		const String DIGITS = "0123456789ABCDEF";
		const Byte ZERO = (Byte)'0';
		const Byte ALPHA_CAP = (Byte)'A';
		const Byte ALPHA_LOW = (Byte)'a';
		const Byte DIV_SHIFT = 4;
		const Byte MOD_AND = 15;

	}
}
