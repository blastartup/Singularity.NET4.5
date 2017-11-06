using System;
using System.Reflection;
using System.Resources;

namespace Singularity.Resources
{
	public class ResourcesManager : ResourceManager
	{
		public ResourcesManager() : base() { }
		public ResourcesManager(Type resourceSource) : base(resourceSource) { }
		public ResourcesManager(String baseName, Assembly assembly) : base(baseName, assembly) { }
		public ResourcesManager(String baseName, Assembly assembly, Type usingResourceSet) : base(baseName, assembly, usingResourceSet) { }

		public String Translate(String sentence)
		{
			return TranslateCore(new Words(sentence));
		}

		public String Translate(Words words)
		{
			return TranslateCore(words);
		}

		private String TranslateCore(Words words)
		{
			for (Int32 idx = 0; idx < words.Count; idx++)
			{
				words[idx] = ResString(words[idx]);
			}
			return words.ToString();
		}


		/// <summary>
		/// <para>Obtains strings from the CmdLineArgResourceManager.</para>
		/// <para>If resource does not exist or ResourceManager has no data assigned,
		/// the supplied ResourceManagerString is returned.</para>
		/// </summary>
		/// <param name="resourceManagerString">Is looked up in ResourceManager</param>
		/// <returns>Match in ResourceManager or if no match available ResourceManagerString</returns>
		private String ResString(String resourceManagerString)
		{
			String result = resourceManagerString;

			if (!result.IsSurroundedBy(ESurroundType.SquareBrackets))
			{
				try
				{
					result = GetString(resourceManagerString);
				}
				catch (MissingManifestResourceException)
				{
					throw new Exception("Internal Exception: MissingManifestResourceException: Make sure NamespaceOfResX passed constructor CLAParser() is correct. If the resx-file is directly included to project, NamespaceOfResX must be the default namespace. In your main file Program.cs look for the line starting with \"namespace\" at top of file and try to pass the string which follows to constructor CLAParser().");
				}
				catch (Exception)
				{
					result = resourceManagerString;
				}

				if (result.IsEmpty())
				{
					result = resourceManagerString;
				}
			}
			return result;
		}
	}
}
