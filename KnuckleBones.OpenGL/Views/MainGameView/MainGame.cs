using KnuckleBones.Core.Engines;
using KnuckleBones.Core.Models.Game.Opponents;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;

namespace KnuckleBones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        public static Guid ID = new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361");

        public KnuckleBonesEngine Engine { get; set; }

        public MainGame(IWindow parent) : base(parent, ID)
        {
            Engine = new KnuckleBonesEngine();
            Initialize();
        }

        public void TakeTurn()
        {
            if (!Engine.TakeTurn())
                return;

            if (Engine.State.FirstOpponent is PlayerOpponent player1)
                player1.SetTargetColumn(-1);
            if (Engine.State.SecondOpponent is PlayerOpponent player2)
                player2.SetTargetColumn(-1);
            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();
        }
    }
}