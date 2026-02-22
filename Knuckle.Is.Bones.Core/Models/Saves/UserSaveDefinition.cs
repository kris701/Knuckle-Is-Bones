using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
	public class UserSaveDefinition<T>
	{
		public int AllTimeScore { get; set; }
		public T UIData { get; set; }

		public UserSaveDefinition(int allTimeScore, T uIData)
		{
			AllTimeScore = allTimeScore;
			UIData = uIData;
		}

		public void Save()
		{
			if (File.Exists("user.json"))
				File.Delete("user.json");
			File.WriteAllText("user.json", JsonSerializer.Serialize(this));
		}
	}
}
