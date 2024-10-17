using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game
{
    public class DiceDefinition : IDefinition, IGenericClonable<DiceDefinition>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sides { get; set; }
        public int Value { get; set; } = 0;

        [JsonConstructor]
        public DiceDefinition(Guid iD, string name, string description, int sides, int value)
        {
            ID = iD;
            Name = name;
            Description = description;
            Sides = sides;
            Value = value;
        }

        public DiceDefinition(DiceDefinition other)
        {
            ID = other.ID;
            Name = other.Name;
            Description = other.Description;
            Sides = other.Sides;
            Value = other.Value;
        }

        public DiceDefinition Clone() => new DiceDefinition(this);
    }
}