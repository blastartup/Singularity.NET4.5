using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
// ReSharper disable ConvertToAutoProperty

namespace Singularity.WinForm
{
	public abstract class BaseView
	{
		public String TitleLabel
		{
			get => _titlelabel;
			set => _titlelabel = value;
		}
		private String _titlelabel;

		public String DescriptionLabel
		{
			get => _descriptionLabel;
			set => _descriptionLabel = value;
		}
		private String _descriptionLabel;

		public Boolean IsLoaded => LoadedDateTime != null;

		public DateTime? LoadedDateTime
		{
			get => _loadedDateTime;
			set
			{
				PreviousLoadedDateTime = _loadedDateTime;
				_loadedDateTime = value;
			}
		}
		protected internal DateTime? PreviousLoadedDateTime;
		private DateTime? _loadedDateTime;
	}
}
