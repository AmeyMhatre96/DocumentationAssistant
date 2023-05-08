using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentationAssistant2;
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
			var headerTemplate = Application.Settings.GetSetting("FileHeader");
			headerTemplate = headerTemplate.Replace("[FileName]", doc.Parent.Name);
			var headerContent = headerTemplate;//ConvertToSummaryComment(headerTemplate);
			SyntaxTrivia newHeaderTrivia = SyntaxFactory.Comment(headerContent);
			SyntaxNode newRoot = root.ReplaceTrivia(header, newHeaderTrivia);

			// Insert the file header at the top of the document
 
			edit.Insert(headerContent);
		}

		public static string ConvertToSummaryComment(string input)
		{
			string[] lines = input.Trim().Split('\n');

			// Trim leading and trailing white spaces from each line
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = lines[i].Trim();
			}

			// Add summary comment tags to each line
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = "* " + lines[i];
			}

			// Join the lines back together with line breaks
			string result = string.Join(Environment.NewLine, lines);

			// Add the summary comment opening and closing tags
			result = "/*" + Environment.NewLine + result + Environment.NewLine + "*/" + Environment.NewLine + Environment.NewLine;

			return result;
		}

	}
}
