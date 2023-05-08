using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DocumentationAssistant
{
	internal class AppSettings
	{
		public static AppSettings Instance { get; set; }

		public Dictionary<string, string> _settings = new Dictionary<string, string>();
		private readonly string _configPath = string.Empty;
		public AppSettings(string localDataPath)
		{
			// Get the path to the user.config file for Visual Studio 2022
			this._configPath = Path.Combine(localDataPath, "DocumentationAssitant", "Settings.json");
			// Load the configuration file
			if (File.Exists(this._configPath))
			{
				string content = File.ReadAllText(this._configPath);
				this._settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
			}
			Instance = this;
		}

		public string GetSetting(string key)
		{
			return this._settings.ContainsKey(key) ? this._settings[key] : string.Empty;
		}

		public void SetSetting(string key, string value)
		{
			this._settings[key] = value;
			var dir = Path.GetDirectoryName(this._configPath);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			File.WriteAllText(this._configPath, JsonConvert.SerializeObject(this._settings));
		}
	}
}
