using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Shell;

namespace DocumentationAssistant.Vsix.Commands.SetFileHeader
{
	internal class FileHeaderGenerator
	{
		public static void AddHeaderInCurrentFile()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			var dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
			if (dte != null)
			{

			}
			var doc = dte.ActiveDocument.Object("TextDocument") as TextDocument;
			var selection = doc.Selection;

			var editPoint = doc.StartPoint.CreateEditPoint();
			string text = editPoint.GetText(doc.EndPoint);

			// Insert the file header at the top of the document
			var startPoint = doc.StartPoint;
			var edit = startPoint.CreateEditPoint();

			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text);

			var trivia = syntaxTree.GetRoot().DescendantTrivia();
			foreach (var item in trivia)
			{

			}
			var root = syntaxTree.GetRoot();
			var header = trivia.FirstOrDefault();
			string newHeader = "/*\r\n" +
					"* Author: New Author Name\r\n" +
					"* Date: " + DateTime.Today.ToString("d MMMM yyyy") + "\r\n" +
					"* Description: Enter description here\r\n" +
					"*/\r\n";
			SyntaxTrivia newHeaderTrivia = SyntaxFactory.Comment(newHeader);
			SyntaxNode newRoot = root.ReplaceTrivia(header, newHeaderTrivia);

			// Insert the file header at the top of the document
 
			edit.Insert(newHeader);
		}
	}
}
