using KnuckleBones.Core.Models.Game.OpponentModules;
using System.Text.Json.Serialization;

namespace KnuckleBones.Core.Models.Game
{
    public class OpponentDefinition : IDefinition, IGenericClonable<OpponentDefinition>
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IOpponentModule Module { get; set; }

        [JsonConstructor]
        public OpponentDefinition(Guid iD, string name, string description, IOpponentModule module)
        {
            ID = iD;
            Name = name;
            Description = description;
            Module = module;
        }

        public OpponentDefinition(OpponentDefinition other)
        {
            ID = other.ID;
            Name = other.Name;
            Description = other.Description;
            if (other.Module.Clone() is IOpponentModule module)
                Module = module;
            else
                throw new ArgumentException("Could not clone module!");
        }

        public OpponentDefinition Clone() => new OpponentDefinition(this);
    }
}