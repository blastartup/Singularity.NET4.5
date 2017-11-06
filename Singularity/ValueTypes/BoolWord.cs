using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	[DebuggerDisplay("{_value}")]
	public struct BoolWord : IComparable, IComparable<BoolWord>, IComparable<Boolean>, IStateEmpty, IStateValid
	{
		[DebuggerStepThrough]
		public BoolWord(Boolean value, EBoolWordStyle wordStyle = EBoolWordStyle.TrueFalse)
		{
			_value = value;
			_default = false;
			_style = wordStyle;
		}

		[DebuggerStepThrough]
		public BoolWord(Char value, EBoolCharStyle charStyle = EBoolCharStyle.Yn)
		{
			_value = value.Equals(charStyle.GetAlternateValue()[0]);
			_default = false;
			_style = (EBoolWordStyle)charStyle;
		}

		[DebuggerStepThrough]
		public BoolWord(String value, EBoolWordStyle wordStyle = EBoolWordStyle.TrueFalse)
		{
			String trueValue = wordStyle.GetAlternateValue().KeepLeft(ValueLib.Comma.CharValue);
			_value = value.Equals(trueValue, StringComparison.OrdinalIgnoreCase);
			_default = false;
			_style = wordStyle;
		}


		#region Object Overrides
		public override Boolean Equals(Object obj)
		{
			return (obj is BoolWord && (BoolWord)obj == this) ||
					(obj is Boolean && (Boolean)obj == _value);
		}

		public override Int32 GetHashCode()
		{
			return _value.GetHashCode();
		}

		public Char ToChar()
		{
			Char result = 'F';
			if (_style < EBoolWordStyle.OnOff)
			{
				String[] values = _style.GetAlternateValue().Split(ValueLib.Comma.CharValue);
				result = values[Convert.ToInt32(!_value)][0];
			}
			return result;
		}

		public override String ToString()
		{
			String[] values = _style.GetAlternateValue().Split(ValueLib.Comma.CharValue);
			return values[Convert.ToInt32(!_value)];
		}

		#endregion

		#region Casting

		[DebuggerStepThrough]
		public static implicit operator BoolWord(Boolean value)
		{
			return value ? True : False;
		}

		[DebuggerStepThrough]
		public static implicit operator Boolean(BoolWord value)
		{
			return value._value;
		}

		#endregion

		#region True/False Constants
		public static readonly BoolWord True = new BoolWord(true);
		public static readonly BoolWord False = new BoolWord(false);
		#endregion

		public Boolean IsEmpty => false;

		public Boolean IsDefault => this == _default;

		public BoolWord Default
		{
			get { return _default; }
			set { _default = value._value; }
		}
		private Boolean _default;

		#region IFStateValid Members

		public Boolean IsValid => true;

		#endregion

		#region IComparable and Generic Members

		public Int32 CompareTo(Object aOther)
		{
			return _value.CompareTo(aOther);
		}

		public Int32 CompareTo(BoolWord aOther)
		{
			return _value.CompareTo(aOther._value);
		}

		public Int32 CompareTo(Boolean aOther)
		{
			return _value.CompareTo(aOther);
		}

		#endregion

		private readonly Boolean _value;
		private readonly EBoolWordStyle _style;
	}
}
