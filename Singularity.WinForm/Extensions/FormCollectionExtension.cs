using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Singularity.WinForm.Extensions
{
	public static class FormCollectionExtension
	{
		public static IEnumerable<Form> AsEnumerable(this FormCollection formCollection)
		{
			foreach (Form form in formCollection)
			{
				yield return form;
			}
		}

		public static Form GetForm(FormCollection formCollection, String formName)
		{
			return formCollection.AsEnumerable().FirstOrDefault(f => f.Name.Equals(formName, StringComparison.OrdinalIgnoreCase));
		}
	}
}
