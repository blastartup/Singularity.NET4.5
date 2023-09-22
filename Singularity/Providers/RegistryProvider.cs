using Singularity.Types;
using System;
using System.IO;

namespace Singularity
{
    public class RegistryProvider
	{
		public RegistryBranch Serialize(FileInfo registryFile)
		{
			if (!registryFile.Exists) throw new ArgumentException($"Registry file ({registryFile}) doesn't exist");

			RegistryBranch registryBranch = new RegistryBranch();
			RegistryNode registryNode = null;
			var lineCounter = -1;
			foreach (String line in File.ReadLines(registryFile.FullName))
            {
                lineCounter++;
				if (lineCounter == 0)
				{
					var words = new Words(line, " ");
					registryBranch.EditorVersion = words.LastWord;
					continue;
				}

				if (line.IsEmpty())
				{
					continue;
				}

				if (line[0] == '[')
				{
					registryNode = new RegistryNode()
					{
						Path = line.Desurround()
					};
					registryBranch.Nodes.Add(registryNode);
					continue;
				}

				if (line[0] == '"')
				{
					var items = new Words(line, "=");
					var registryItem = new RegistryItem()
					{
						Key = items[0].Desurround(),
						Value = items[1].Desurround()
					};
					registryNode?.Items.Add(registryItem);
					continue;
				}
				else
				{
					var items = new Words(line, "=");
					var registryItem = new RegistryItem()
					{
						Key = items[0],
						Value = items[1].Desurround()
					};
					registryNode?.Items.Add(registryItem);
					continue;
				}
			}

            return registryBranch;
        }
	}
}
