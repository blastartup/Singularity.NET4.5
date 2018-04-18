using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Singularity.DataService.Extensions
{
	public static class XmlReaderExtension
	{
		public static Boolean IsEndElement(this XmlReader xmlReader, String name = "")
		{
			if (xmlReader.NodeType == XmlNodeType.EndElement)
			{
				if (name == String.Empty)
				{
					return true;
				}

				return (xmlReader.Name == name);
			}

			return false;
		}
	}
}
