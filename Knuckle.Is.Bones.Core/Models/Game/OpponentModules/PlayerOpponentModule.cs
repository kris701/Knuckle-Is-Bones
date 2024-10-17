using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
    public class PlayerOpponentModule : IOpponentModule
    {
        public Guid OpponentID { get; set; } = Guid.NewGuid();

        private int _targetColumn = 0;

        [JsonConstructor]
        public PlayerOpponentModule(Guid opponentID)
        {
            OpponentID = opponentID;
        }

        public void SetTargetColumn(BoardDefinition board) => throw new NotImplementedException();

        public void SetTargetColumn(int targetColumn) => _targetColumn = targetColumn;

        public int GetTargetColumn() => _targetColumn;

        public IOpponentModule Clone() => new PlayerOpponentModule(OpponentID);
    }
}