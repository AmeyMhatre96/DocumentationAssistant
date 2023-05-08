namespace DocumentationAssistant.Helper
{
	public static class Configuration
	{
		/// <summary>
		/// Gets a value indicating whether the tool is enabled for only public members.
		/// </summary>
		public static bool IsEnabledForPublicMembersOnly { get; set; } = true;

		public static bool ExcludeSuggestionsForTestProjects { get; set; } = true;
	}
}
