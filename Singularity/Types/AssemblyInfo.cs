using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

// ReSharper disable once CheckNamespace

namespace Singularity
{
	/// <summary>
	/// A wrapper class around Assembly, presenting a cleaner interface of assembly properties.
	/// </summary>
	public class AssemblyInfo
	{
		/// <summary>
		/// Primary constructor requires an assembly object.
		/// </summary>
		/// <param name="assembly">A given assembly of which to wrap around.</param>
		public AssemblyInfo(Assembly assembly)
		{
			_assembly = assembly;
		}

		private T CustomAttributes<T>() where T : Attribute
		{
			object[] customAttributes = _assembly.GetCustomAttributes(typeof(T), false);

			if (customAttributes.Length > 0)
			{
				return ((T)customAttributes[0]);
			}

			throw new InvalidOperationException();
		}

		/// <summary>
		/// An assemblies Global Unique Identifier.
		/// </summary>
		public Guid Guid => new Guid(CustomAttributes<System.Runtime.InteropServices.GuidAttribute>().Value);

		/// <summary>
		/// Assembly title.
		/// </summary>
		public String Title => CustomAttributes<AssemblyTitleAttribute>().Title;

		/// <summary>
		/// Description of assembly.
		/// </summary>
		public String Description => CustomAttributes<AssemblyDescriptionAttribute>().Description;

		/// <summary>
		/// Company maker of assembly.
		/// </summary>
		public String Company => CustomAttributes<AssemblyCompanyAttribute>().Company;

		/// <summary>
		/// Product name of assembly.
		/// </summary>
		public String Product => CustomAttributes<AssemblyProductAttribute>().Product;

		/// <summary>
		/// Assemblies configuration details.
		/// </summary>
		public String Configuration => CustomAttributes<AssemblyConfigurationAttribute>().Configuration;

		/// <summary>
		/// Assemblies copyright details.
		/// </summary>
		public String Copyright => CustomAttributes<AssemblyCopyrightAttribute>().Copyright;

		/// <summary>
		/// Assemblies trademark.
		/// </summary>
		public String Trademark => CustomAttributes<AssemblyTrademarkAttribute>().Trademark;

		/// <summary>
		/// Assemblies version.
		/// </summary>
		public String AssemblyVersion => _assembly.GetName().Version.ToString();

		/// <summary>
		/// Assemblies file version.
		/// </summary>
		public String FileVersion => FileVersionInfo.GetVersionInfo(_assembly.Location).FileVersion;

		/// <summary>
		/// Assemblies file name.
		/// </summary>
		public String FileName => FileVersionInfo.GetVersionInfo(_assembly.Location).OriginalFilename;

		/// <summary>
		/// Assemblies file path.
		/// </summary>
		public String FilePath => FileVersionInfo.GetVersionInfo(_assembly.Location).FileName;

		/// <summary>
		/// Local application data folder
		/// </summary>
		public DirectoryInfo ApplicationBaseFolder
		{
			get
			{
				if (_applicationBaseFolder == null)
				{
					_applicationBaseFolder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
						$"{Company}\\{Product}"));
					if (!_applicationBaseFolder.Exists)
					{
						_applicationBaseFolder.Create();
					}
				}
				return _applicationBaseFolder;
			}
		}

		private DirectoryInfo _applicationBaseFolder;

		public DirectoryInfo ApplicationDataFolder
		{
			get 
			{
				if (_applicationDataFolder == null)
				{
					_applicationDataFolder = new DirectoryInfo(Path.Combine(ApplicationBaseFolder.FullName, "Data"));
					if (!_applicationDataFolder.Exists)
					{
						_applicationDataFolder.Create();
					}
				}
				return _applicationDataFolder;
			}
		}

		private DirectoryInfo _applicationDataFolder;

		public DirectoryInfo ApplicationTempFolder
		{
			get 
			{
				if (_applicationTempFolder == null)
				{
					_applicationTempFolder = new DirectoryInfo(Path.Combine(ApplicationBaseFolder.FullName, "Temp"));
					if (!_applicationTempFolder.Exists)
					{
						_applicationTempFolder.Create();
					}
				}
				return _applicationTempFolder;
			}
		}

		private DirectoryInfo _applicationTempFolder;

		public ManifestResourceInfo GetEmbeddedResourceInfo(String resourceName)
		{
			return _assembly.GetManifestResourceInfo(resourceName);
		}

		public Stream GetEmbeddedResourceStream(String resourceName)
		{
			return _assembly.GetManifestResourceStream(resourceName);
		}

		public Stream GetEmbeddedResourceStream(Type resourceType, String resourceName)
		{
			return _assembly.GetManifestResourceStream(resourceType, resourceName);
		}

		private readonly Assembly _assembly;
	}
}
