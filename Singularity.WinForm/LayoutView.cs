using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
// ReSharper disable ConvertToAutoProperty

namespace Singularity.WinForm
{
	public abstract class LayoutView : BaseView
	{
		public String FirstHiglightTitleLabel
		{
			get => _firstHiglightTitleLabel;
			set => _firstHiglightTitleLabel = value;
		}
		private String _firstHiglightTitleLabel;

		public String SecondHiglightTitleLabel
		{
			get => _secondHiglightTitleLabel;
			set => _secondHiglightTitleLabel = value;
		}
		private String _secondHiglightTitleLabel;

		public String BodyTitleLabel
		{
			get => _bodyTitleLabel;
			set => _bodyTitleLabel = value;
		}
		private String _bodyTitleLabel;

		public String FirstHiglightCountLabel
		{
			get => _firstHiglightCountLabel;
			set => _firstHiglightCountLabel = value;
		}
		private String _firstHiglightCountLabel;

		public String SecondHiglightCountLabel
		{
			get => _secondHiglightCountLabel;
			set => _secondHiglightCountLabel = value;
		}
		private String _secondHiglightCountLabel;

		public Boolean IsReloaded => IsLoaded && LoadedDateTime != PreviousLoadedDateTime;
	}
}
