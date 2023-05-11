using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace DocumentationAssistant.Helper
{
	public class TestsProjectChecker
	{
		public static bool IsTestProject(SemanticModel model)
		{
			// Get the Compilation from the SemanticModel
			Compilation compilation = model.Compilation;

			// Get the Assembly from the Compilation
			var assembly = compilation.Assembly;

			// Check the name of the project associated with the Assembly
			string projectName = assembly.Name;

			if (projectName.EndsWith(".Tests"))
			{
				return true;
			}
			return false;
		}
	}
}