using Knuckle.Is.Bones.Core.Engines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Helpers
{
	public static class GameSaveHelpers
	{
		public static string SaveFileName { get; } = "save.json";
		public static bool DoesSaveExist() => File.Exists(SaveFileName);
		public static void DeleteSave()
		{
			if (File.Exists(SaveFileName))
				File.Delete(SaveFileName);
		}
		public static GameState? LoadSaveFile()
		{
			if (!File.Exists(SaveFileName))
				return null;
			var item = JsonSerializer.Deserialize<GameState>(File.ReadAllText(SaveFileName));
			return item;
		}
		public static void Save(GameState state)
		{
			if (File.Exists(SaveFileName))
				File.Delete(SaveFileName);
			File.WriteAllText(SaveFileName, JsonSerializer.Serialize(state));
		}
	}
}
