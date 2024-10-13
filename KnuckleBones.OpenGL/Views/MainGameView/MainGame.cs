using KnuckleBones.Core.Engines;
using KnuckleBones.Core.Models.Game.Opponents;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Input;
using MonoGame.OpenGL.Formatter.Views;
using System;

namespace KnuckleBones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        public static Guid ID = new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361");

        public KnuckleBonesEngine Engine { get; set; }
        private KeyWatcher _leftKeyWatcher;
        private KeyWatcher _rightKeyWatcher;
        private KeyWatcher _enterKeyWatcher;

        public MainGame(IWindow parent) : base(parent, ID)
        {
            Engine = new KnuckleBonesEngine();
            _leftKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Left, MoveLeft);
            _rightKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Right, MoveRight);
            _enterKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Enter, TakeTurn);
            Initialize();
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            _leftKeyWatcher.Update(keyState);
            _rightKeyWatcher.Update(keyState);
            _enterKeyWatcher.Update(keyState);
        }

        private void TakeTurn()
        {
            if (!Engine.TakeTurn())
                return;
            if (Engine.GameOver)
                ShowGameOverView();

            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();

            if (Engine.GetCurrentOpponent() is not PlayerOpponent)
                TakeTurn();
        }

        private void MoveLeft()
        {
            var opponent = Engine.GetCurrentOpponent();
            if (opponent is PlayerOpponent player)
            {
                var board = Engine.GetCurrentOpponentBoard();
                var current = player.GetTargetColumn(board);
                current--;
                if (current < 0)
                    current = 0;
                player.SetTargetColumn(current);
            }
            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();
        }

        private void MoveRight()
        {
            var opponent = Engine.GetCurrentOpponent();
            if (opponent is PlayerOpponent player)
            {
                var board = Engine.GetCurrentOpponentBoard();
                var current = player.GetTargetColumn(board);
                current++;
                if (current >= board.Columns.Count)
                    current = board.Columns.Count - 1;
                player.SetTargetColumn(current);
            }
            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();
        }
    }
}