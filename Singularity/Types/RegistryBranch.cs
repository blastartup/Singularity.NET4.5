using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Types
{
	public class RegistryBranch
	{
		public RegistryBranch()
		{
			Nodes = new HashSet<RegistryNode>();
		}

		public String EditorVersion { get; set; }
		public HashSet<RegistryNode> Nodes { get; }

	}
}
