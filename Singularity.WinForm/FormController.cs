using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Singularity.WinForm
{
	public abstract class FormController
	{
		protected FormController(IWin32Window owner)
		{
			_owner = owner;
		}

		public Boolean BrowseFolder(FolderBrowserDialog folderBrowserDialog, TextBox textbox, String description)
		{
			folderBrowserDialog.Description = description;
			if (textbox.Text.IsEmpty())
			{
				folderBrowserDialog.SelectedPath = textbox.Text;
			}

			DialogResult result = folderBrowserDialog.ShowDialog(Owner);
			if (result == DialogResult.OK)
			{
				textbox.Text = folderBrowserDialog.SelectedPath;
				return true;
			}
			return false;
		}

		public String GetConnectedString(String originalString)
		{
			//var dataConnectionDialog = new DataConnectionDialog();
			return String.Empty;
		}

		public static IEnumerable<T> GetControlsOfType<T>(Control root)
			 where T : Control
		{
			T t = root as T;
			if (t != null)
			{
				yield return t;
			}

			foreach (Control c in root.Controls)
			{
				foreach (T i in GetControlsOfType<T>(c))
				{
					yield return i;
				}
			}
		}

		protected IWin32Window Owner => _owner;
		private readonly IWin32Window _owner;
	}


	public abstract class FormController<TModel> : FormController
	{
		protected FormController(IWin32Window owner) : base(owner)
		{
		}

		public abstract TModel FormRead();
		public abstract void FormUpdate(TModel model);

	}
}
