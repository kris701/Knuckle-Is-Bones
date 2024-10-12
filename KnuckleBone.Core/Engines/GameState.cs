using KnuckleBones.Core.Models.Game;
using KnuckleBones.Core.Models.Game.Opponents;

namespace KnuckleBones.Core.Engines
{
    public class GameState
    {
        public Guid Turn { get; set; }
        public IOpponent FirstOpponent { get; set; }
        public BoardDefinition FirstOpponentBoard { get; set; }
        public IOpponent SecondOpponent { get; set; }
        public BoardDefinition SecondOpponentBoard { get; set; }

        public DiceDefinition? CurrentDice { get; set; }
    }
}