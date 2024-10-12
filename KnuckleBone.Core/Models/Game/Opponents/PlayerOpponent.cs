namespace KnuckleBones.Core.Models.Game.Opponents
{
    public class PlayerOpponent : BaseOpponent
    {
        private int _targetColumn = -1;

        public PlayerOpponent(Guid iD, string name, string description) : base(iD, name, description)
        {
        }

        public PlayerOpponent(Guid iD, string name, string description, Guid opponentID) : base(iD, name, description, opponentID)
        {
        }

        public void SetTargetColumn(int targetColumn) => _targetColumn = targetColumn;

        public override int GetTargetColumn(BoardDefinition board) => _targetColumn;
    }
}