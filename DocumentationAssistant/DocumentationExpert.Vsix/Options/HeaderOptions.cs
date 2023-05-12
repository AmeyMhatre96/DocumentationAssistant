using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static DocumentationExpert.Vsix.Options.OptionsProvider;

namespace DocumentationExpert.Vsix.Options
{
	internal class HeaderOptionsControl : UserControl
	{
		private RichTextBox _fileHeaderContentTextBox;
		private readonly HeaderOptionsPage _headerOptionsPage;

		public HeaderOptionsControl(HeaderOptionsPage headerOptionsPage)
		{
			this._headerOptionsPage = headerOptionsPage;
			InitializeComponent();
			InitializeComponents();
		}

		private void InitializeComponents()
		{
			this._fileHeaderContentTextBox = new RichTextBox();
			this._fileHeaderContentTextBox.Dock = DockStyle.Top;
			this._fileHeaderContentTextBox.Height = 200;
			this._fileHeaderContentTextBox.Width = 200;
			this._fileHeaderContentTextBox.Margin = new Padding(10);
			this._fileHeaderContentTextBox.Text = this._headerOptionsPage.FileHeaderText;
			this._fileHeaderContentTextBox.Multiline = true;
			this._fileHeaderContentTextBox.KeyDown += RichTextBox_KeyDown;
			this._fileHeaderContentTextBox.KeyUp += this._richTextBox_KeyUp;
			this._fileHeaderContentTextBox.TextChanged += this.RichTextBox_TextChanged;
			this._fileHeaderContentTextBox.BorderStyle = BorderStyle.FixedSingle;

			var textHint = new TextBox();
			textHint.Enabled = false;
			textHint.Text = "Enter the header content. \n\rAvailable variables: {fileName}, {date}";

			TableLayoutPanel layoutPanel = new TableLayoutPanel();
			layoutPanel.Dock = DockStyle.Fill;
			layoutPanel.Controls.Add(textHint);
			layoutPanel.Controls.Add(this._fileHeaderContentTextBox);

			this.Controls.Add(layoutPanel);

			// Bind the custom text property to the value of the textbox
			//_FileHeaderContentTextBox.DataBindings.Add("Text", _optionPage, nameof(_optionPage.FileHeaderText));
		}

		private void RichTextBox_TextChanged(object sender, EventArgs e)
		{
			this._headerOptionsPage.FileHeaderText = this._fileHeaderContentTextBox.Text;
		}

		private void _richTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			// Check if the Enter key was pressed
			if (e.KeyCode == Keys.Enter)
			{
				// Prevent the default behavior of the Enter key
				e.Handled = true;
				e.SuppressKeyPress = true;

				// Insert a new line character at the current caret position
				_fileHeaderContentTextBox.SelectedText = Environment.NewLine;
			}
		}

		private void RichTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			// Check if the Enter key was pressed
			if (e.KeyCode == Keys.Enter)
			{
				// Prevent the default behavior of the Enter key
				e.Handled = true;
				e.SuppressKeyPress = true;

				// Insert a new line character at the current caret position
				_fileHeaderContentTextBox.SelectedText = Environment.NewLine;
			}
			//_optionPage.FileHeaderText = _FileHeaderContentTextBox.Text;
		}

		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // HeaderOptionsControl
            // 
            this.Name = "HeaderOptionsControl";
            this.Load += new System.EventHandler(this.HeaderOptionsControl_Load);
            this.ResumeLayout(false);

		}

		private void HeaderOptionsControl_Load(object sender, EventArgs e)
		{

		}
	}
}
