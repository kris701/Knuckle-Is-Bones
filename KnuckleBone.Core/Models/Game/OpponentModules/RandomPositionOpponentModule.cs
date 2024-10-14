namespace KnuckleBones.Core.Models.Game.OpponentModules
{
    public class RandomPositionOpponentModule : IOpponentModule
    {
        public Guid OpponentID { get; set; }

        private readonly Random _rnd = new Random();
        private int _targetColumn = 0;

        public RandomPositionOpponentModule(Guid opponentID)
        {
            OpponentID = opponentID;
        }

        public RandomPositionOpponentModule()
        {
            OpponentID = Guid.NewGuid();
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

        public IOpponentModule Clone() => new RandomPositionOpponentModule();
    }
}