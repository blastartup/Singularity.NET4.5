using System;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum EBoolWordStyle
	{
		[EnumAdditional("True or False", alternateValue:"True,False")]
		TrueFalse,
		[EnumAdditional("T or F", alternateValue: "True,False")]
		Tf = 1,
		[EnumAdditional("Yes or No", alternateValue: "Yes,No")]
		YesNo,
		[EnumAdditional("Y or N", alternateValue: "Y,N")]
		Yn,
		[EnumAdditional("+ or -", alternateValue: "+,-")]
		PlusMinus,
		[EnumAdditional("On or Off", alternateValue: "On,Off")]
		OnOff,
		[EnumAdditional("Positive or Negative", alternateValue: "Positive,Negative")]
		PositiveNegative,
		[EnumAdditional("Up or Down", alternateValue: "Up,Down")]
		UpDown,
		[EnumAdditional("Right or Left", alternateValue: "Right,Left")]
		RightLeft,
		[EnumAdditional("Open or Close", alternateValue: "Open,Close")]
		OpenClose
	}

	public enum EBoolCharStyle
	{
		[EnumAdditional("Y or N", alternateValue: "Y,N")]
		Tf = 1,
		[EnumAdditional("Y or N", alternateValue: "Y,N")]
		Yn = 3,
		[EnumAdditional("+ or -", alternateValue: "+,-")]
		PlusMinus = 4,
	}
}
