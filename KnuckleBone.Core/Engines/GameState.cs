using KnuckleBone.Core.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBone.Core.Engines
{
    public enum GameResult
    { None, Lost, Success }

    public enum GameTurn
    { None, Player, Opponent }

    public class GameState
    {
        public GameTurn Turn { get; set; }
        public BoardDefinition PlayerBoard { get; set; }
        public BoardDefinition OpponentBoard { get; set; }

        public DiceDefinition CurrentDice { get; set; }
    }
}