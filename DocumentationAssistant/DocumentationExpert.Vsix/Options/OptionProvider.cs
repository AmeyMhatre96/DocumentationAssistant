using System.Runtime.InteropServices;

namespace DocumentationExpert.Vsix.Options
{
	internal partial class OptionsProvider
	{
		// Register the options with this attribute on your package class:
		// [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "DocumentationExpert.Vsix", "General", 0, 0, true, SupportsProfiles = true)]
		[ComVisible(true)]
		public class GeneralOptions : BaseOptionPage<General>
		{
		}
	}
}
