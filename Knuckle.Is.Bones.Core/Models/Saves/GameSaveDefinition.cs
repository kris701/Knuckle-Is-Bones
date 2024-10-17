using Knuckle.Is.Bones.Core.Engines;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Saves
{
    public class GameSaveDefinition : IGenericClonable<GameSaveDefinition>
    {
        public DateTime SaveTime { get; set; }
        public GameState State { get; set; }

        [JsonConstructor]
        public GameSaveDefinition(DateTime saveTime, GameState state)
        {
            SaveTime = saveTime;
            State = state;
        }

        public GameSaveDefinition(GameState state)
        {
            SaveTime = DateTime.Now;
            State = state;
        }

        public GameSaveDefinition(string save) : this(JsonSerializer.Deserialize<GameSaveDefinition>(File.ReadAllText("save.json")))
        {
        }

        public GameSaveDefinition(GameSaveDefinition other)
        {
            SaveTime = other.SaveTime;
            State = other.State.Clone();
        }

        public void Save()
        {
            if (File.Exists("save.json"))
                File.Delete("save.json");
            this.SaveTime = DateTime.Now;
            File.WriteAllText("save.json", JsonSerializer.Serialize(this));
        }

        public GameSaveDefinition Clone() => new GameSaveDefinition(this);
    }
}
