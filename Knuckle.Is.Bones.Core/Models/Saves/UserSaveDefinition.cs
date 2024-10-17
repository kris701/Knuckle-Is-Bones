using System.Text.Json;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
    public class UserSaveDefinition
    {
        public int AllTimeScore { get; set; }

        public void Save()
        {
            if (File.Exists("user.json"))
                File.Delete("user.json");
            File.WriteAllText("user.json", JsonSerializer.Serialize(this));
        }
    }
}
