using System.Text.Json;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
	public class UserSaveDefinition<T> where T : new()
	{
		public int AllTimeScore { get; set; } = 0;
		public Dictionary<Guid, int> CompletedItems { get; set; } = new Dictionary<Guid, int>();
		public T UIData { get; set; } = new T();

		public void Save()
		{
			if (File.Exists("user.json"))
				File.Delete("user.json");
			File.WriteAllText("user.json", JsonSerializer.Serialize(this));
		}

		public void AppendCompletedItem(Guid id)
		{
			if (CompletedItems.ContainsKey(id))
				CompletedItems[id]++;
			else
				CompletedItems.Add(id, 1);
		}
	}
}
