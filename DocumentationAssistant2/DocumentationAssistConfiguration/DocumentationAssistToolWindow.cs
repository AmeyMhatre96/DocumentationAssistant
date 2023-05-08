using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DocumentationAssistant2.DocumentationAssistConfiguration
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	/// </summary>
	/// <remarks>
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
	/// usually implemented by the package implementer.
	/// <para>
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its
	/// implementation of the IVsUIElementPane interface.
	/// </para>
	/// </remarks>
	[Guid("fb2472b3-0c1f-4e6d-910a-5def1171be61")]
	public class DocumentationAssistToolWindow : ToolWindowPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentationAssistToolWindow"/> class.
		/// </summary>
		public DocumentationAssistToolWindow() : base(null)
		{
			this.Caption = "DocumentationAssist Configurations";

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
			// the object returned by the Content property.
			Action close = () => ((IVsWindowFrame)this.Frame).CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_PromptSave);	
			this.Content = new DocumentationAssistToolWindowControl(close);
		}
	}
}
