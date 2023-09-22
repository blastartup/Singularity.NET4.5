using System;
using System.Collections.Generic;

namespace Singularity.Types
{
	public class RegistryNode
	{
		public RegistryNode()
		{
			Items = new HashSet<RegistryItem>();
		}

		public String Path { get; set; }
		public HashSet<RegistryItem> Items { get; }
	}
}