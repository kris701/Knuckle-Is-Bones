namespace KnuckleBones.Core.Models.Game.Opponents
{
    public class RandomPositionOpponent : BaseOpponent
    {
        private readonly Random _rnd = new Random();

        public RandomPositionOpponent(Guid iD, string name, string description) : base(iD, name, description)
        {
        }

        public RandomPositionOpponent(Guid iD, string name, string description, Guid opponentID) : base(iD, name, description, opponentID)
        {
        }

        public override int GetTargetColumn(BoardDefinition board)
        {
            int target = -1;
            bool valid = false;
            while (!valid)
            {
                target = _rnd.Next(0, board.Columns.Count);
                if (!board.Columns[target].IsFull())
                    valid = true;
            }
            return target;
        }
    }
}