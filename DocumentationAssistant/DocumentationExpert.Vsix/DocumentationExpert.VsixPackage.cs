global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using System;

global using Task = System.Threading.Tasks.Task;

using System.Runtime.InteropServices;
using System.Threading;

using static DocumentationExpert.Vsix.Options.OptionsProvider;

namespace DocumentationExpert.Vsix
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideOptionPage(typeof(Options.OptionsProvider.GeneralOptions), "Documentation Expert", "General", 0, 0, true, SupportsProfiles = true)]
	[ProvideOptionPage(typeof(Options.OptionsProvider.HeaderOptionsPage), "Documentation Expert", "File header", 0, 0, true, SupportsProfiles = true)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.DocumentationExpertVsixString)]
	public sealed class DocumentationExpertVsixPackage : ToolkitPackage
	{
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{

			await this.RegisterCommandsAsync();
		}

		internal HeaderOptionsPage HeaderOptions
		{
			get
			{
				HeaderOptionsPage page = (HeaderOptionsPage)GetDialogPage(typeof(HeaderOptionsPage));
				return page;
			}
		}
	}
}