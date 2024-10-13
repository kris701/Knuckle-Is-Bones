using KnuckleBones.Core.Engines;
using KnuckleBones.Core.Models.Game.Opponents;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Input;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Threading;
using KnuckleBones.Core.Helpers;

namespace KnuckleBones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        public static Guid ID = new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361");

        public KnuckleBonesEngine Engine { get; set; }
        private KeyWatcher _leftKeyWatcher;
        private KeyWatcher _rightKeyWatcher;
        private KeyWatcher _enterKeyWatcher;
        private GameTimer _rollTimer;
        private bool _controlsLocked = true;
        private bool _rolling = true;
        private int _rolledTimes = 0;
        private Random _rnd = new Random();

        public MainGame(IWindow parent) : base(parent, ID)
        {
            Engine = new KnuckleBonesEngine();
            _leftKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Left, MoveLeft);
            _rightKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Right, MoveRight);
            _enterKeyWatcher = new KeyWatcher(Microsoft.Xna.Framework.Input.Keys.Enter, TakeTurn);
            _rollTimer = new GameTimer(TimeSpan.FromMilliseconds(75), (x) =>
            {
                UpdateDice(_rnd.Next(1, Engine.State.CurrentDice.Sides + 1));
                _rolledTimes++;
                if (_rolledTimes > 10)
                {
                    _rolledTimes = 0;
                    _rolling = false;
                    UpdateDice(Engine.State.CurrentDice.Value);

                    if (Engine.GetCurrentOpponent() is not PlayerOpponent)
                        TakeTurn();
                    else
                    {
                        _controlsLocked = false;
                        UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
                        UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
                    }
                }
            });
            Initialize();
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            _leftKeyWatcher.Update(keyState);
            _rightKeyWatcher.Update(keyState);
            _enterKeyWatcher.Update(keyState);
            if (_rolling)
                _rollTimer.Update(gameTime.ElapsedGameTime);
        }

        private void TakeTurn()
        {
            if (!Engine.TakeTurn())
                return;
            _controlsLocked = true;

            if (Engine.GameOver)
            {
                ShowGameOverView();
                return;
            }

            _rolling = true;

            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
        }

        private void MoveLeft()
        {
            if (_controlsLocked)
                return;
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
        }

        private void MoveRight()
        {
            if (_controlsLocked)
                return;
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
        }
    }
}