using KnuckleBones.Core.Models.Game;

namespace KnuckleBones.Core.Engines
{
    public class GameState
    {
        public Guid Winner { get; set; }
        public Guid Turn { get; set; }
        public OpponentDefinition FirstOpponent { get; set; }
        public BoardDefinition FirstOpponentBoard { get; set; }
        public OpponentDefinition SecondOpponent { get; set; }
        public BoardDefinition SecondOpponentBoard { get; set; }

        public DiceDefinition CurrentDice { get; set; }
    }
}