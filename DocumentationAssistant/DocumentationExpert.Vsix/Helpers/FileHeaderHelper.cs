using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationExpert.Vsix.Helpers
{
	internal static class FileHeaderHelper
	{
		public static int GetHeaderLength(string text)
		{
			var headerLength = GetHeaderLength(text, "//") + GetHeaderLength(text, "/*", "*/");
			return headerLength;
		}

		private static int GetHeaderLength(string text, string commentSyntax)
		{
			if (!text.TrimStart().StartsWith(commentSyntax))
			{
				return 0;
			}

			var lines = SplitLines(text);
			var header = new List<string>();

			header.AddRange(GetEmptyLines(lines, 0));
			header.AddRange(GetLinesStartingWith(commentSyntax, lines, header.Count));

			var nbChar = 0;

			if (header.Count == 0)
			{
				return 0;
			}

			header.ToList().ForEach(x => nbChar += x.Length + 2);

			return nbChar;
		}

		private static int GetHeaderLength(string text, string commentSyntaxStart, string commentSyntaxEnd)
		{
			if (!text.TrimStart().StartsWith(commentSyntaxStart) || text.IndexOf(commentSyntaxEnd) == -1)
			{
				return 0;
			}

			var lines = SplitLines(text);
			var header = new List<string>();

			header.AddRange(GetEmptyLines(lines, 0));

			foreach (var line in lines.Skip(header.Count))
			{
				header.Add(line);

				if (line.TrimEnd().EndsWith(commentSyntaxEnd))
				{
					break;
				}
			}

			var nbChar = 0;

			if (header.Count == 0)
			{
				return 0;
			}

			foreach (var h in header)
			{
				nbChar += h.Length + 2;
			}

			return nbChar;
		}

		private static IEnumerable<string> GetLinesStartingWith(string pattern, IEnumerable<string> lines, int nbLinesToSkip = 0)
		{
			List<string> result = new List<string>();

			foreach (var line in lines.Skip(nbLinesToSkip))
			{
				if (!line.StartsWith(pattern))
				{
					break;
				}

				result.Add(line);
			}

			return result;
		}

		private static IEnumerable<string> GetEmptyLines(IEnumerable<string> lines, int nbLinesToSkip)
		{
			List<string> result = new List<string>();

			foreach (var line in lines.Skip(nbLinesToSkip))
			{
				if (!string.IsNullOrWhiteSpace(line))
				{
					break;
				}

				result.Add(line);
			}

			return result;
		}


		private static IEnumerable<string> SplitLines(string text)
		{
			var separator = new string[] { Environment.NewLine };

			return text.Split(separator, StringSplitOptions.None);
		}
	}
}
