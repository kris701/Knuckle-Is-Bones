using Knuckle.Is.Bones.Core.Models.Saves;
using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Helpers
{
	public class UserSaveHelpers
	{
		public static string SaveFileName { get; } = "user.json";
		public static void Save(UserSaveDefinition user)
		{
			if (File.Exists(SaveFileName))
				File.Delete(SaveFileName);
			File.WriteAllText(SaveFileName, JsonSerializer.Serialize(user));
		}
		public static UserSaveDefinition? LoadSaveFile()
		{
			if (!File.Exists(SaveFileName))
				return null;
			var item = JsonSerializer.Deserialize<UserSaveDefinition>(File.ReadAllText(SaveFileName));
			return item;
		}
	}
}
