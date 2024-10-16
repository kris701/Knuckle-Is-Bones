namespace KnuckleBones.Core.Models.Game.OpponentModules
{
    public class PlayerOpponentModule : IOpponentModule
    {
        public Guid OpponentID { get; set; }

        private int _targetColumn = 0;

        public PlayerOpponentModule(Guid opponentID)
        {
            OpponentID = opponentID;
        }

        public PlayerOpponentModule()
        {
            OpponentID = Guid.NewGuid();
        }

        public void SetTargetColumn(BoardDefinition board) => throw new NotImplementedException();

        public void SetTargetColumn(int targetColumn) => _targetColumn = targetColumn;

        public int GetTargetColumn() => _targetColumn;

        public IOpponentModule Clone() => new PlayerOpponentModule();
    }
}