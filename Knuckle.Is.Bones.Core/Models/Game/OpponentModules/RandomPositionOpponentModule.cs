using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
    public class RandomPositionOpponentModule : IOpponentModule
    {
        public Guid OpponentID { get; set; } = Guid.NewGuid();

        private readonly Random _rnd = new Random();
        private int _targetColumn = 0;

        [JsonConstructor]
        public RandomPositionOpponentModule(Guid opponentID)
        {
            OpponentID = opponentID;
        }

        public void SetTargetColumn(BoardDefinition board)
        {
            int target = -1;
            bool valid = false;
            while (!valid)
            {
                target = _rnd.Next(0, board.Columns.Count);
                if (!board.Columns[target].IsFull())
                    valid = true;
            }
            _targetColumn = target;
        }

        public int GetTargetColumn() => _targetColumn;

        public IOpponentModule Clone() => new RandomPositionOpponentModule(OpponentID);
    }
}