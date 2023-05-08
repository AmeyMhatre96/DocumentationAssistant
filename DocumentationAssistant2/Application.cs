using System;
using System.Threading;
using System.Threading.Tasks;
using DocumentationAssistant.Helper;
using Microsoft.VisualStudio.Shell;

namespace DocumentationAssistant2
{
	internal class Application
	{
		private static AsyncPackage _package;

		internal static void Initialize(AsyncPackage package)
		{
			_package = package;

			// Init configuration
			if (string.IsNullOrEmpty(Settings.GetSetting("EnforceOnlyForPublicMembers")))
			{
				Settings.SetSetting("EnforceOnlyForPublicMembers", "true");
			}
			Configuration.IsEnabledForPublicMembersOnly = bool.Parse(Settings.GetSetting("EnforceOnlyForPublicMembers"));
		}

		private static AppSettings _appSettings;

		public static AppSettings Settings
		{
			get
			{
				if (_appSettings == null)
				{
					_appSettings = new AppSettings(_package);
				}
				return _appSettings;
			}
		}

		public static T GetService<T>()
		{
			return (T)_package.GetServiceAsync(typeof(T)).Result;
		}

		public static void AddService<T>(T t)
		{
			Task<object> CreateMyServiceAsync(IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
			{
				return Task.FromResult<object>(t);
			}
			_package.AddService(typeof(T), CreateMyServiceAsync);
		}
	}
}
