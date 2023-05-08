using System;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DocumentationAssistant2.DocumentationAssistConfiguration
{
	/// <summary>
	/// Interaction logic for DocumentationAssistToolWindowControl.
	/// </summary>
	public partial class DocumentationAssistToolWindowControl : UserControl
	{
		private readonly Action _close;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentationAssistToolWindowControl"/> class.
		/// </summary>
		public DocumentationAssistToolWindowControl(System.Action close)
		{
			this.InitializeComponent();

			TextPointer insertionPosition = this.HeaderNameEditor.Selection.Start;

			// Get the selected item from the ListView

			// Create a new TextRange object with the insertion position
			TextRange range = new TextRange(insertionPosition, insertionPosition);

			// Set the text of the TextRange to the selected item
			range.Text = Application.Settings.GetSetting("FileHeader");
			this._close = close;
		}

		private void ListView_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var source = e.Source as System.Windows.Controls.ListViewItem;
			if (source != null)
			{
				TextPointer insertionPosition = this.HeaderNameEditor.Selection.Start;

				// Get the selected item from the ListView
				string selectedItem = source.Content as string;

				// Create a new TextRange object with the insertion position
				TextRange range = new TextRange(insertionPosition, insertionPosition);

				// Set the text of the TextRange to the selected item
				range.Text = $"[{selectedItem}]";
			}
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			TextRange textRange = new TextRange(this.HeaderNameEditor.Document.ContentStart, this.HeaderNameEditor.Document.ContentEnd);
			string text = textRange.Text;

			// Get the selected item from the ListView

			Application.Settings.SetSetting("FileHeader", text);

			this._close();
		}
	}
}