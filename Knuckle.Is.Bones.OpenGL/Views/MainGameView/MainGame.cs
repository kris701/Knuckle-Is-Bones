using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Game.OpponentModules;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Input;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.IO;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseKnuckleBoneFadeView
    {
        public static Guid ID = new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361");

        public KnuckleBonesEngine Engine { get; set; }
        public GameSaveDefinition Save { get; set; }

        private readonly KeyWatcher _leftKeyWatcher;
        private readonly KeyWatcher _rightKeyWatcher;
        private readonly KeyWatcher _enterKeyWatcher;
        private readonly KeyWatcher _escapeKeyWatcher;
        private readonly GameTimer _rollTimer;
        private readonly GameTimer _rollWaitTimer;
        private readonly GameTimer _selectWaitTimer;
        private bool _selectWait = false;
        private bool _rolling = true;
        private bool _rollWait = false;
        private int _movePosition = 0;
        private int _rolledTimes = 0;
        private readonly Random _rnd = new Random();

        public MainGame(KnuckleBoneWindow parent, GameSaveDefinition save) : base(parent, ID)
        {
            Save = save;
            Engine = new KnuckleBonesEngine(save);

            _leftKeyWatcher = new KeyWatcher(Keys.Left, MoveLeft);
            _rightKeyWatcher = new KeyWatcher(Keys.Right, MoveRight);
            _enterKeyWatcher = new KeyWatcher(Keys.Enter, TakeTurn);
            _escapeKeyWatcher = new KeyWatcher(Keys.Escape, Escape);
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
                    current.Module.SetTargetColumn(Engine.State.CurrentDice, Engine.GetCurrentOpponentBoard(), Engine.GetNextOpponentBoard());
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
            _escapeKeyWatcher.Update(keyState);
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

                var pointsGained = 0;
                if ((Engine.State.FirstOpponent.Module is PlayerOpponentModule) && (Engine.State.SecondOpponent.Module is not PlayerOpponentModule))
                    pointsGained = (int)(Engine.State.FirstOpponentBoard.GetValue() * Engine.State.SecondOpponent.Difficulty);
                if ((Engine.State.SecondOpponent.Module is PlayerOpponentModule) && (Engine.State.FirstOpponent.Module is PlayerOpponentModule))
                    pointsGained = (int)(Engine.State.SecondOpponentBoard.GetValue() * Engine.State.FirstOpponent.Difficulty);

                Parent.User.AllTimeScore += pointsGained;
                Parent.User.Save();
                if (pointsGained > 0)
                    _pointsGainedLabel.Text = $"Gained {pointsGained} points.";
                else
                    _pointsGainedLabel.Text = "No points awarded";

                if (Engine.State.Winner == Engine.State.FirstOpponent.Module.OpponentID)
                    _winnerLabel.Text = "First Opponent Won!";
                else
                    _winnerLabel.Text = "Second Opponent Won!";
                _gameOverPanel.IsVisible = true;
                if (File.Exists("save.json"))
                    File.Delete("save.json");

                return;
            }

            _rolling = true;
            UpdateFirstOpponentBoard();
            UpdateSecondOpponentBoard();
        }

        private void MoveLeft()
        {
            if (_rolling || _rollWait || _selectWait)
                return;

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
            if (_rolling || _rollWait || _selectWait)
                return;

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

        private void Escape()
        {
            if ((Engine.State.FirstOpponent.Module is PlayerOpponentModule) || (Engine.State.SecondOpponent.Module is PlayerOpponentModule))
                if (_rolling || _rollWait || _selectWait)
                    return;
            SwitchView(new MainMenu(Parent));
        }
    }
}