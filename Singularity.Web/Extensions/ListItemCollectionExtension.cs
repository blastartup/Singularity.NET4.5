using System;
using System.Text;
using System.Web.UI.WebControls;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class ListItemCollectionExtension
	{
		public static String ContentsToString(this ListItemCollection collection)
		{
			String format = "Text={0};Value={1};Selected={2};Enabled={3}|";
			StringBuilder result = new StringBuilder();
			foreach (ListItem item in collection)
			{
				result.AppendFormat(format, item.Text, item.Value, item.Selected ? "Yes" : "No", item.Enabled ? "Yes" : "No");
			}
			return result.ToString();
		}
	}
}

//#region Test
//#if DEBUG
//namespace Singularity.Web.Testing
//{
//   using NUnit.Framework;
//   using System.Collections.Generic;

//   [TestFixture]
//   public class ListItemCollectionExtensionTest
//   {
//      [TestCase]
//      public void TestContentsToString()
//      {
//         ListItemCollection collection = new ListItemCollection();
//         Assert.IsTrue(collection.ContentsToString() == "", "Empty dictionary should return empty result not => \"{0}\".".FormatX(collection.ToString()));

//         collection.Add(new ListItem("text1", "value1", true));
//         collection.Add(new ListItem("text2", "value2", false));
//         collection[1].Selected = true;
//         Assert.IsTrue(collection.ContentsToString() == "Text=text1;Value=value1;Selected=No;Enabled=Yes|Text=text2;Value=value2;Selected=Yes;Enabled=No|", "Populated dictionary should return expected result not => \"{0}\".".FormatX(collection.ContentsToString()));
//      }
//   }
//}
//#endif
//#endregion
