using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
	public class UserSaveDefinition
	{
		public int Points { get; set; } = 0;
		public Dictionary<Guid, int> CompletedItems { get; set; } = new Dictionary<Guid, int>();
		public List<Guid> PurchasedShopItems { get; set; } = new List<Guid>();

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

		public int GetCompletedTimes(Guid id)
		{
			if (!CompletedItems.ContainsKey(id))
				CompletedItems.Add(id, 0);
			return CompletedItems[id];
		}
	}
}
