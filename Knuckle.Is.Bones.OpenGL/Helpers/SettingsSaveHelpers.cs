using Knuckle.Is.Bones.OpenGL.Models;
using System.IO;
using System.Text.Json;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class SettingsSaveHelpers
	{
		public static string SaveFileName { get; } = "settings.json";
		public static void Save(SettingsDefinition settings)
		{
			if (File.Exists(SaveFileName))
				File.Delete(SaveFileName);
			File.WriteAllText(SaveFileName, JsonSerializer.Serialize(settings));
		}
		public static SettingsDefinition? LoadSaveFile()
		{
			if (!File.Exists(SaveFileName))
				return null;
			var item = JsonSerializer.Deserialize<SettingsDefinition>(File.ReadAllText(SaveFileName));
			return item;
		}
	}
}
