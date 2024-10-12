using KnuckleBone.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnuckleBone.Core.Engines
{
    public delegate void GameEventHandler();

    public class KnuckleBonesEngine
    {
        public GameEventHandler? OnGameOver;
        public GameEventHandler? OnBoardChanged;

        public GameState State { get; }
        public GameResult Result { get; set; } = GameResult.None;
        public bool GameOver { get; set; }

        public KnuckleBonesEngine()
        {
            State = new GameState();
        }

        public void PlaceDice()
        {
        }
    }
}