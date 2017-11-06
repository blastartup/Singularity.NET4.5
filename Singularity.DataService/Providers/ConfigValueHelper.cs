using System;

// ReSharper disable once CheckNamespace

namespace Singularity.DataService
{
	public abstract class ConfigValueHelper<T> where T : IConvertible
	{
		protected ConfigValueHelper(IConfiguration defaultConfiguration, IFormatProvider formatProvider = null)
		{
			_defaultConfiguration = defaultConfiguration;
			_formatProvider = formatProvider;
		}

		public T Value
		{
			get
			{
				IConfiguration configuration = GetField();
				if (configuration == null)
				{
					T defaultValue = default(T);
					CreateDefaultValue(defaultValue);
					Save();
					return defaultValue;
				}

				String value = configuration.Value;
				if (value == null)
				{
					return default(T);
				}

				if (_formatProvider == null)
				{
					return (T)Convert.ChangeType(value, typeof (T));
				}
				return (T)Convert.ChangeType(value, typeof (T), _formatProvider);
			}
			set
			{
				SetValue(value, true);
			}
		}

		public void SetValue(T value, Boolean saveNow)
		{
			IConfiguration field = GetField();
			if (field == null)
			{
				throw new ArgumentException("Cannot update field ({0}) as it is not defined in the database and no default provided at initialization".FormatX(DefaultConfiguration.ConfigurationId));
			}
			field.Value = _formatProvider == null ? value.ToString() : value.ToString(_formatProvider);
			if (saveNow)
			{
				Save();
			}
		}

		protected abstract void CreateDefaultValue(T defaultValue);

		protected abstract IConfiguration GetField();
		protected abstract void Save();

		protected IConfiguration DefaultConfiguration
		{
			get { return _defaultConfiguration; }
		}
		private readonly IConfiguration _defaultConfiguration;

		private readonly IFormatProvider _formatProvider;
	}
}