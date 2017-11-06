using System;
using System.IO;
using System.Text;
using System.Web.UI;

// ReSharper disable once CheckNamespace

namespace Singularity.Web
{
	public static class ControlExtension
	{
		public static String ToHtml(this Control selectedControl)
		{
			StringBuilder sb = new StringBuilder();
			using (StringWriter sw = new StringWriter(sb))
			{
				using (HtmlTextWriter textWriter = new HtmlTextWriter(sw))
				{
					try { selectedControl.RenderControl(textWriter); }
					catch (System.Web.HttpException) { }
				}
			}
			return sb.ToString();
		}

	}
}
