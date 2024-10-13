using KnuckleBones.Core.Engines;
using KnuckleBones.Core.Models.Game.Opponents;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Input;
using MonoGame.OpenGL.Formatter.Views;
using System;
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
        private GameTimer _moveTimer;
        private bool _controlsLocked = true;
        private bool _rolling = true;
        private int _rolledTimes = 0;
        private bool _moving = false;
        private Point _origin;
        private Point _target;
        private Random _rnd = new Random();

        public MainGame(IWindow parent) : base(parent, ID)
        {
            Engine = new KnuckleBonesEngine();
            _leftKeyWatcher = new KeyWatcher(Keys.Left, MoveLeft);
            _rightKeyWatcher = new KeyWatcher(Keys.Right, MoveRight);
            _enterKeyWatcher = new KeyWatcher(Keys.Enter, TakeTurn);
            _rollTimer = new GameTimer(TimeSpan.FromMilliseconds(50), (x) =>
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
            _moveTimer = new GameTimer(TimeSpan.FromMilliseconds(33), (x) =>
            {
                var current = new Point((int)_diceLabel.X, (int)_diceLabel.Y);
                if (current.X == _target.X && current.Y == _target.Y)
                {
                    _moving = false;

                    UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
                    UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);

                    if (Engine.GameOver)
                    {
                        ShowGameOverView();
                        return;
                    }

                    _diceLabel.X = _origin.X;
                    _diceLabel.Y = _origin.Y;
                    _rolling = true;
                }
                var dir = _target.Y - _diceLabel.Y;
                if (dir > 0)
                    _diceLabel.Y += 10;
                else
                    _diceLabel.Y -= 10;
                _diceLabel.Initialize();
            });
            Initialize();

            _origin = new Point((int)_diceLabel.X, (int)_diceLabel.Y);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            _leftKeyWatcher.Update(keyState);
            _rightKeyWatcher.Update(keyState);
            _enterKeyWatcher.Update(keyState);
            if (_rolling)
                _rollTimer.Update(gameTime.ElapsedGameTime);
            if (_moving)
                _moveTimer.Update(gameTime.ElapsedGameTime);
        }

        private void TakeTurn()
        {
            if (Engine.State.Turn == Engine.State.FirstOpponent.OpponentID)
                _target = new Point(_origin.X, _origin.Y - 100);
            else
                _target = new Point(_origin.X, _origin.Y + 100);

            if (!Engine.TakeTurn())
                return;
            _controlsLocked = true;
            _moving = true;
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