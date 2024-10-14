namespace KnuckleBones.Core.Models.Game.Opponents
{
    public class PlayerOpponent : BaseOpponent
    {
        public PlayerOpponent(Guid iD, string name, string description) : base(iD, name, description)
        {
        }

        public PlayerOpponent(Guid iD, string name, string description, Guid opponentID) : base(iD, name, description, opponentID)
        {
        }

        public void SetTargetColumn(int targetColumn) => _targetColumn = targetColumn;
    }
}