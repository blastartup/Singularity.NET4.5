// ReSharper disable once CheckNamespace

namespace Singularity.Win32API
{
	#region Objects needed for the Win32 functions
	#pragma warning disable 1591

	/// <summary>
	/// The resource display type.
	/// </summary>
	public enum ResourceDisplaytype : int
	{
		Generic = 0x0,
		Domain = 0x01,
		Server = 0x02,
		Share = 0x03,
		File = 0x04,
		Group = 0x05,
		Network = 0x06,
		Root = 0x07,
		Shareadmin = 0x08,
		Directory = 0x09,
		Tree = 0x0a,
		Ndscontainer = 0x0b
	}

	#pragma warning restore 1591
	#endregion
}
