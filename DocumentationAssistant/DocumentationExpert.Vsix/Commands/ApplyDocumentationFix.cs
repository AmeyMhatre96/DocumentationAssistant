using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DocumentationAssistant;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

namespace DocumentationExpert.Vsix
{
	/// <summary>
	/// The apply documentation fix.
	/// </summary>
	[Command(PackageIds.ApplyDocumentationFix)]
	internal sealed class ApplyDocumentationFix : BaseCommand<ApplyDocumentationFix>
	{
		/// <inheritdoc/>
		protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
		{
			// Get the component model from the global service provider
			Dictionary<string, IDocumentationFixProvider> documentationFixProviders = await GetDocumentationFixProvidersAsync();

			DocumentView documentView = await VS.Documents.GetActiveDocumentViewAsync();
			var textSnapshot = documentView.TextView.TextSnapshot;
			var text = textSnapshot.GetText();
			SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text);
			var rootNode = await syntaxTree.GetRootAsync();
			Dictionary<SyntaxNode, SyntaxNode> newNodesToReplace = new Dictionary<SyntaxNode, SyntaxNode>();

			// We have to traverse the tree bottom-up, because we want to replace the nodes from the bottom up
			async Task<SyntaxNode> TraverseAndCollect(SyntaxNode node, SyntaxNode parent)
			{
				var updatedNode = node;
				// Recursively traverse the child nodes,
				var listOfChildren = updatedNode.ChildNodes().ToList();
				for (int i = 0; i < listOfChildren.Count; i++)
				{
					updatedNode = await TraverseAndCollect(listOfChildren[i], updatedNode);
					listOfChildren = updatedNode.ChildNodes().ToList();
				}

				if (documentationFixProviders.TryGetValue(updatedNode.GetType().FullName, out var codeFixProvider))
				{
					DocumentationCommentTriviaSyntax commentTriviaSyntax = updatedNode
						.GetLeadingTrivia()
						.Select(o => o.GetStructure())
						.OfType<DocumentationCommentTriviaSyntax>()
						.FirstOrDefault();

					if (commentTriviaSyntax == null)
					{
						var newDeclaration = await codeFixProvider.AddDocumentationHeaderAsync(updatedNode, CancellationToken.None);
						if (newDeclaration != null)
						{
							return parent.ReplaceNode(node, newDeclaration);
						}
						newNodesToReplace.Add(node, newDeclaration);
					}
				}

				return parent.ReplaceNode(node, updatedNode);
			}

			SyntaxNode updatedRootNode = rootNode;
			List<SyntaxNode> listOfChildren = updatedRootNode.ChildNodes().ToList();
			for (int i = 0; i < listOfChildren.Count; i++)
			{
				updatedRootNode = await TraverseAndCollect(listOfChildren[i], updatedRootNode);
				listOfChildren = updatedRootNode.ChildNodes().ToList();
			}

			using (var edit = documentView.Document.TextBuffer.CreateEdit())
			{
				edit.Replace(0, documentView.Document.TextBuffer.CurrentSnapshot.Length, updatedRootNode.GetText().ToString());
				edit.Apply();
			}

			// Set file header for document
			await VS.Commands.ExecuteAsync(PackageGuids.DocumentationExpertVsix, PackageIds.SetFileHeader);

			// Format the document
			await VS.Commands.ExecuteAsync(VSConstants.VSStd2K, (int)VSConstants.VSStd2KCmdID.FORMATDOCUMENT);
		}

		/// <summary>
		/// Gets the documentation fix providers async.
		/// </summary>
		/// <returns>A System.Threading.Tasks.Task&lt;Dictionary&lt;string, IDocumentationFixProvider&gt;&gt;.</returns>
		private async Task<Dictionary<string, IDocumentationFixProvider>> GetDocumentationFixProvidersAsync()
		{
			// using reflection, get all the classes that are extending DocumentationFixProvider as base class
			// Get the assembly where the DocumentationFixProvider is defined
			Assembly assembly = typeof(DocumentationFixProvider<>).Assembly;

			// Get all types in the assembly that are assignable to DocumentationFixProvider
			List<string> fixProviderTypes = assembly.GetTypes()
				.Where(type => type.IsSubclassOf(typeof(CodeFixProvider)))
				.Select(t => t.Name)
				.ToList();

			IComponentModel componentModel = (await this.Package.GetServiceAsync(typeof(SComponentModel))) as IComponentModel;
			var compositionService = componentModel.DefaultExportProvider;
			var exports = compositionService.GetExports<CodeFixProvider, IDictionary<string, object>>();
			var codeFixProviders = new List<CodeFixProvider>();
			foreach (var export in exports)
			{
				var metadata = export.Metadata;
				if (metadata.TryGetValue("Name", out var nameObj) && nameObj is string name && fixProviderTypes.Contains(name))
				{
					var codeFixProvider = export.Value;
					codeFixProviders.Add(codeFixProvider);
				}
			}

			var documentationFixProviders = new Dictionary<string, IDocumentationFixProvider>();
			foreach (IDocumentationFixProvider fixProvider in codeFixProviders.OfType<IDocumentationFixProvider>())
			{
				var argumentType = fixProvider.GetType().BaseType.GetGenericArguments().FirstOrDefault();
				if (argumentType != null)
				{
					documentationFixProviders.Add(argumentType.FullName, fixProvider);
				}
			}

			return documentationFixProviders;
		}
	}
}
