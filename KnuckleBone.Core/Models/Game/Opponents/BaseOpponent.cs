using System.Text.Json.Serialization;

namespace KnuckleBones.Core.Models.Game.Opponents
{
    public abstract class BaseOpponent : IOpponent
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid OpponentID { get; set; }

        internal int _targetColumn = 0;

        public BaseOpponent(Guid iD, string name, string description)
        {
            ID = iD;
            Name = name;
            Description = description;
            OpponentID = Guid.NewGuid();
        }

        [JsonConstructor]
        protected BaseOpponent(Guid iD, string name, string description, Guid opponentID) : this(iD, name, description)
        {
            OpponentID = opponentID;
        }

        public int GetTargetColumn() => _targetColumn;
    }
}