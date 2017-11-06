using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	public enum EFixedResponseTypes
	{
		/// <summary>
		/// Unknown configuration setting.
		/// </summary>
		/// <remarks>Unknown response type</remarks>
		[EnumAdditional("Unknown", "UNK", "Unknown response type")]
		Unknown = 0,

		/// <summary>
		/// Message configuration setting.
		/// </summary>
		/// <remarks>User message</remarks>
		[EnumAdditional("Message", "MSG", "User message")]
		Message = 1,

		/// <summary>
		/// Error configuration setting.
		/// </summary>
		/// <remarks>Error condition</remarks>
		[EnumAdditional("Error", "ERR", "Error condition")]
		Error = 2
	}
}
