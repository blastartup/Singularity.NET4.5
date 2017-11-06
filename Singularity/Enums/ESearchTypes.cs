
// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum ESearchTypes
	{
		[EnumAdditional("Exactly", "IS", "Search is...")]
		Exactly = 0,
		[EnumAdditional("Starts With", "STR", "Search starting with...")]
		StartsWith = 1,
		[EnumAdditional("Ends With", "END", "Search ends with...")]
		EndsWith = 2,
		[EnumAdditional("Contains", "CONT", "Search containing...")]
		Contains = 3,
	}
}