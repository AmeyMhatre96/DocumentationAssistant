using System.Runtime.InteropServices;
using System.Windows.Forms;

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

		[ComVisible(true)]
		[Guid("4DDC4CD8-60F9-4170-8E98-AB639AC5ED9F")]
		public class HeaderOptionsPage : DialogPage
		{
			public string FileHeaderText { get; set; }

			protected override System.Windows.Forms.IWin32Window Window
			{
				get
				{
					HeaderOptionsControl page = new HeaderOptionsControl(this);
					return page;
				}
			}
		}
	}
}
