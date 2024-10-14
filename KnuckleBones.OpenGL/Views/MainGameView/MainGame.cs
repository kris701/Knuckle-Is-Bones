using KnuckleBones.Core.Engines;
using KnuckleBones.Core.Helpers;
using KnuckleBones.Core.Models.Game.OpponentModules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private readonly KeyWatcher _leftKeyWatcher;
        private readonly KeyWatcher _rightKeyWatcher;
        private readonly KeyWatcher _enterKeyWatcher;
        private readonly GameTimer _rollTimer;
        private readonly GameTimer _rollWaitTimer;
        private readonly GameTimer _selectWaitTimer;
        private bool _selectWait = false;
        private bool _rolling = true;
        private bool _rollWait = false;
        private int _movePosition = 0;
        private int _rolledTimes = 0;
        private readonly Random _rnd = new Random();

        public MainGame(IWindow parent) : base(parent, ID)
        {
            Engine = new KnuckleBonesEngine();
            _leftKeyWatcher = new KeyWatcher(Keys.Left, MoveLeft);
            _rightKeyWatcher = new KeyWatcher(Keys.Right, MoveRight);
            _enterKeyWatcher = new KeyWatcher(Keys.Enter, TakeTurn);
            _rollTimer = new GameTimer(TimeSpan.FromMilliseconds(50), (x) =>
            {
                _diceLabel.Text = $"{_rnd.Next(1, Engine.State.CurrentDice.Sides + 1)}";
                _rolledTimes++;
                if (_rolledTimes > 10)
                {
                    _rolledTimes = 0;
                    _rolling = false;
                    _rollWait = true;
                    _diceLabel.Text = $"{Engine.State.CurrentDice.Value}";
                }
            });
            _rollWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(750), (x) =>
            {
                _rollWait = false;
                _diceLabel.Text = $"{Engine.State.CurrentDice.Value}";

                var current = Engine.GetCurrentOpponent();
                if (current.Module is not PlayerOpponentModule)
                {
                    _selectWait = true;
                    _movePosition = current.Module.GetTargetColumn();
                    current.Module.SetTargetColumn(Engine.GetCurrentOpponentBoard());
                }
                UpdateFirstOpponentBoard();
                UpdateSecondOpponentBoard();
            });
            _selectWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(750), (x) =>
            {
                _selectWait = false;
                TakeTurn();
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
            if (_rollWait)
                _rollWaitTimer.Update(gameTime.ElapsedGameTime);
            if (_selectWait)
                _selectWaitTimer.Update(gameTime.ElapsedGameTime);
        }

        private void TakeTurn()
        {
            if (_rolling || _rollWait || _selectWait)
                return;

            if (!Engine.TakeTurn())
                return;

            if (Engine.GameOver)
            {
                UpdateFirstOpponentBoard();
                UpdateSecondOpponentBoard();

                if (Engine.State.Winner == Engine.State.FirstOpponent.Module.OpponentID)
                    _winnerLabel.Text = "Player won!";
                else
                    _winnerLabel.Text = "CPU won!";
                _gameOverPanel.IsVisible = true;

                return;
            }

            _rolling = true;
            UpdateFirstOpponentBoard();
            UpdateSecondOpponentBoard();
        }

        private void MoveLeft()
        {
            var opponent = Engine.GetCurrentOpponent();
            if (opponent.Module is PlayerOpponentModule player && Engine.State.Turn == player.OpponentID)
            {
                var board = Engine.GetCurrentOpponentBoard();
                var current = player.GetTargetColumn();
                current--;
                if (current < 0)
                    current = 0;
                player.SetTargetColumn(current);
            }

            if (Engine.State.Turn == Engine.State.FirstOpponent.Module.OpponentID)
                _board1.HighlightColumn(Engine.State.FirstOpponent.Module.GetTargetColumn());
            if (Engine.State.Turn == Engine.State.SecondOpponent.Module.OpponentID)
                _board2.HighlightColumn(Engine.State.SecondOpponent.Module.GetTargetColumn());
        }

        private void MoveRight()
        {
            var opponent = Engine.GetCurrentOpponent();
            if (opponent.Module is PlayerOpponentModule player && Engine.State.Turn == player.OpponentID)
            {
                var board = Engine.GetCurrentOpponentBoard();
                var current = player.GetTargetColumn();
                current++;
                if (current >= board.Columns.Count)
                    current = board.Columns.Count - 1;
                player.SetTargetColumn(current);
            }

            if (Engine.State.Turn == Engine.State.FirstOpponent.Module.OpponentID)
                _board1.HighlightColumn(Engine.State.FirstOpponent.Module.GetTargetColumn());
            if (Engine.State.Turn == Engine.State.SecondOpponent.Module.OpponentID)
                _board2.HighlightColumn(Engine.State.SecondOpponent.Module.GetTargetColumn());
        }
    }
}