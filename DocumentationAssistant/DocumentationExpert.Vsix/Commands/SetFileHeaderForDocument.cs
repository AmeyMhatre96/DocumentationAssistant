
namespace DocumentationExpert.Vsix
{
	[Command(PackageIds.SetFileHeader)]
	internal sealed class SetFileHeaderForDocument : BaseCommand<SetFileHeaderForDocument>
	{
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			DocumentView textView = await VS.Documents.GetActiveDocumentViewAsync();

			var general = await General.GetLiveInstanceAsync();

			var headerToSet = general.FileHeaderText;
			headerToSet = headerToSet.Replace("{fileName}", new System.IO.FileInfo(textView.FilePath).Name);
			headerToSet = headerToSet.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
			headerToSet += Environment.NewLine;
			var textSnapshot = textView.TextView.TextSnapshot;
			var text = textSnapshot.GetText();
			var lengthOfCurrentHeader = Helpers.FileHeaderHelper.GetHeaderLength(text);
			if (lengthOfCurrentHeader == 0)
			{
				textView.TextBuffer.Insert(0, headerToSet);
			}
			else
			{
				if (general.ShouldReplaceFileHeader)
				{
					textView.TextBuffer.Replace(new Microsoft.VisualStudio.Text.Span(0, lengthOfCurrentHeader), headerToSet);
				}
			}
		}
	}
}
