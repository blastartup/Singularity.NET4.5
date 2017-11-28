using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Singularity.WinForm
{
	public abstract class BaseView : INotifyPropertyChanging, INotifyPropertyChanged
	{
		public String TitleLabel
		{
			get => _titlelabel;
			set => SetField(ref _titlelabel, value);
		}
		private String _titlelabel;

		public String DescriptionLabel
		{
			get => _descriptionLabel;
			set => SetField(ref _descriptionLabel, value);
		}
		private String _descriptionLabel;

		public String FirstHiglightTitleLabel
		{
			get => _firstHiglightTitleLabel;
			set => SetField(ref _firstHiglightTitleLabel, value);
		}
		private String _firstHiglightTitleLabel;

		public String SecondHiglightTitleLabel
		{
			get => _secondHiglightTitleLabel;
			set => SetField(ref _secondHiglightTitleLabel, value);
		}
		private String _secondHiglightTitleLabel;

		public String BodyTitleLabel
		{
			get => _bodyTitleLabel;
			set => SetField(ref _bodyTitleLabel, value);
		}
		private String _bodyTitleLabel;

		public String FirstHiglightCountLabel
		{
			get => _firstHiglightCountLabel;
			set => SetField(ref _firstHiglightCountLabel, value);
		}
		private String _firstHiglightCountLabel;

		public String SecondHiglightCountLabel
		{
			get => _secondHiglightCountLabel;
			set => SetField(ref _secondHiglightCountLabel, value);
		}
		private String _secondHiglightCountLabel;

		protected void SetField<T>(ref T field, T value, [CallerMemberName] String propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) return;
			NotifyPropertyChanging(propertyName);
			field = value;
			NotifyPropertyChanged(propertyName);
			ModifiedDateTime = Clock.Now.DateTime;
		}

		protected void NotifyPropertyChanging(String propertyName)
		{
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
		}

		protected void NotifyPropertyChanged(String propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Boolean IsLoaded => LoadedDateTime != null;
		public Boolean IsReloaded => IsLoaded && LoadedDateTime != _previousLoadedDateTime;
		public Boolean IsModified => IsLoaded && ModifiedDateTime != _previousModifiedDateTime;

		public DateTime? LoadedDateTime
		{
			get => _loadedDateTime;
			set
			{
				_previousLoadedDateTime = _loadedDateTime;
				_loadedDateTime = value;
			}
		}
		private DateTime? _loadedDateTime;
		private DateTime? _previousLoadedDateTime;

		public DateTime? ModifiedDateTime
		{
			get => _modifiedDateTime;
			set
			{
				_previousModifiedDateTime = _modifiedDateTime;
				_modifiedDateTime = value;
			}
		}
		private DateTime? _modifiedDateTime;
		private DateTime? _previousModifiedDateTime;

		public event PropertyChangingEventHandler PropertyChanging;
		public event PropertyChangedEventHandler PropertyChanged;

	}
}
