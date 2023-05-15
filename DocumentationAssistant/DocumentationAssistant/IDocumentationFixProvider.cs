using System.Threading.Tasks;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace DocumentationAssistant
{
	/// <summary>
	/// The code syntax fix provider.
	/// </summary>
	public interface IDocumentationFixProvider
	{
		/// <summary>
		/// Adds the documentation header async.
		/// </summary>
		/// <param name="declarationSyntax">The declaration syntax.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		Task<SyntaxNode> AddDocumentationHeaderAsync(SyntaxNode declarationSyntax, CancellationToken cancellationToken);
	}

	/// <summary>
	/// The documentation fix provider.
	/// </summary>
	public abstract class DocumentationFixProvider<DeclarationSyntax> : CodeFixProvider, IDocumentationFixProvider where DeclarationSyntax : SyntaxNode
	{
		/// <summary>
		/// Adds the documentation header async.
		/// </summary>
		/// <param name="declarationSyntax">The declaration syntax.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public async Task<SyntaxNode> AddDocumentationHeaderAsync(SyntaxNode declarationSyntax, CancellationToken cancellationToken)
		{
			return await AddDocumentationHeaderAsync(declarationSyntax as DeclarationSyntax, cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Adds the documentation header async.
		/// </summary>
		/// <param name="declarationSyntax">The declaration syntax.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A Task.</returns>
		public abstract Task<SyntaxNode> AddDocumentationHeaderAsync(DeclarationSyntax declarationSyntax, CancellationToken cancellationToken);
	}
}
