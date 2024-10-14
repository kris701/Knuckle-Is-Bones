namespace KnuckleBones.Core.Models.Game.Opponents
{
    public interface IOpponent : IDefinition
    {
        public Guid OpponentID { get; set; }

        public int GetTargetColumn();
    }
}