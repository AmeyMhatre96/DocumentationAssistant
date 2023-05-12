
using static DocumentationExpert.Vsix.Options.OptionsProvider;

namespace DocumentationExpert.Vsix
{
	[Command(PackageIds.SetFileHeader)]
	internal sealed class SetFileHeaderForDocument : BaseCommand<SetFileHeaderForDocument>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			DocumentView textView = await VS.Documents.GetActiveDocumentViewAsync();

			var general = await General.GetLiveInstanceAsync();

			DocumentationExpertVsixPackage myToolsOptionsPackage = this.Package as DocumentationExpertVsixPackage;

			HeaderOptionsPage page = myToolsOptionsPackage.HeaderOptions;

			var headerToSet = page.FileHeaderText;
			headerToSet = headerToSet.Replace("{fileName}", new System.IO.FileInfo(textView.FilePath).Name);
			headerToSet = headerToSet.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
			if (general.NewLineAfterHeader)
			{
				headerToSet += Environment.NewLine;
			}

			textView.TextBuffer.Insert(0, headerToSet);
		}
	}

	[Command(PackageIds.ReplaceFileHeader)]
	internal sealed class ReplaceFileHeaderForDocument : BaseCommand<ReplaceFileHeaderForDocument>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			DocumentView textView = await VS.Documents.GetActiveDocumentViewAsync();


			var general = await General.GetLiveInstanceAsync();

			DocumentationExpertVsixPackage myToolsOptionsPackage = this.Package as DocumentationExpertVsixPackage;

			HeaderOptionsPage page = myToolsOptionsPackage.HeaderOptions;

			var headerToSet = page.FileHeaderText;
			headerToSet = headerToSet.Replace("{fileName}", new System.IO.FileInfo(textView.FilePath).Name);
			headerToSet = headerToSet.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
			if (general.NewLineAfterHeader)
			{
				headerToSet += Environment.NewLine;
			}

			var textSnapshot = textView.TextView.TextSnapshot;
			var text = textSnapshot.GetText();
			var lengthOfCurrentHeader = Helpers.FileHeaderHelper.GetHeaderLength(text);
			textView.TextBuffer.Replace(new Microsoft.VisualStudio.Text.Span(0, lengthOfCurrentHeader), headerToSet);
		}
	}
}
