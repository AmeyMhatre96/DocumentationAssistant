using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DocumentationExpert.Vsix
{
	


	//[ComVisible(true)]
	//[Guid("4DDC4CD8-60F9-4170-8E98-AB639AC5ED9F")]
	//public class HeaderOptionsPage : DialogPage
	//{
	//	private string _fileHeader = "alpha";

	//	public string FileHeaderText
	//	{
	//		get { return _fileHeader; }
	//		set { _fileHeader = value; }
	//	}

	//	protected override IWin32Window Window
	//	{
	//		get
	//		{
	//			HeaderOptionsControl page = new HeaderOptionsControl(this);
	//			return page;
	//		}
	//	}
	//}



	public class General : BaseOptionModel<General>
	{
		[Category("General")]
		[DisplayName("Header content")]
		[Description("Enter the header content. \n\rAvailable variables: {fileName}, {date}")]
		[DefaultValue("")]
		public string HeaderText { get; set; }

		[Category("General")]
		[DisplayName("Insert new line after header?")]
		[Description("Should a new line be added after the file header content.")]
		[DefaultValue(true)]
		public bool NewLineAfterHeader { get; set; }
	}
}
