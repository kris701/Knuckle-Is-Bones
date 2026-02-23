using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Input;
using System;
using System.Collections.Generic;
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
		private readonly GameTimer _pointsGainedTimer;
		private bool _selectWait = false;
		private bool _rolling = true;
		private bool _rollWait = false;
		private int _rolledTimes = 0;
		private readonly Random _rnd = new Random();
		private Guid _rollSoundEffect = Guid.Empty;

		private readonly List<LabelControl> _pointsGainedControls = new List<LabelControl>();

		public MainGame(KnuckleBoneWindow parent, GameSaveDefinition save) : base(parent, ID)
		{
			Save = save;
			Engine = new KnuckleBonesEngine(save);
			Engine.OnOpponentDiceRemoved += () => Parent.Audio.PlaySoundEffectOnce(new Guid("4e53cd32-7af6-47a1-a331-ec2096505c78"));
			Engine.OnCombo += () => Parent.Audio.PlaySoundEffectOnce(new Guid("74ea48c8-cb6f-4a22-8226-e5d6142b1f76"));
			Engine.OnTurn += () => Parent.Audio.PlaySoundEffectOnce(new Guid("23ac297f-3e68-461f-a869-a304e89e18c6"));

			_leftKeyWatcher = new KeyWatcher(Keys.Left, MoveLeft);
			_rightKeyWatcher = new KeyWatcher(Keys.Right, MoveRight);
			_enterKeyWatcher = new KeyWatcher(Keys.Enter, TakeTurn);
			_escapeKeyWatcher = new KeyWatcher(Keys.Escape, Escape);
			_rollTimer = new GameTimer(TimeSpan.FromMilliseconds(50), OnRollTimer);
			_rollWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(750), OnRollWaitTimer);
			_selectWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(750), OnSelectWaitTimer);
			_pointsGainedTimer = new GameTimer(TimeSpan.FromMilliseconds(100), OnPointsGainedTimer);
			Initialize();
		}

		private void OnRollTimer(TimeSpan span)
		{
			if (_rollSoundEffect == Guid.Empty)
			{
				_rollSoundEffect = Parent.Audio.PlaySoundEffect(new Guid("adb4826c-ae62-4785-b0f3-81dd4d692920"));
			}
			_diceLabel.Text = $"{_rnd.Next(1, Engine.State.CurrentDice.Sides + 1)}";
			_rolledTimes++;
			if (_rolledTimes > 10)
			{
				_rolledTimes = 0;
				_rolling = false;
				_rollWait = true;
				Parent.Audio.StopSoundEffect(_rollSoundEffect);
				_rollSoundEffect = Guid.Empty;
				_diceLabel.Text = $"{Engine.State.CurrentDice.Value}";
			}
		}

		private void OnRollWaitTimer(TimeSpan span)
		{
			_rollWait = false;
			_diceLabel.Text = $"{Engine.State.CurrentDice.Value}";

			var current = Engine.GetCurrentOpponent();
			if (current.MoveModule is not PlayerMoveModule)
			{
				_selectWait = true;
				current.MoveModule.SetTargetColumn(Engine.State.CurrentDice, Engine.GetCurrentOpponentBoard(), Engine.GetNextOpponentBoard());
			}
			UpdateColumnHighlight();
		}

		private void OnSelectWaitTimer(TimeSpan span)
		{
			_selectWait = false;
			TakeTurn();
		}

		private void OnPointsGainedTimer(TimeSpan span)
		{
			var toRemove = new List<LabelControl>();
			foreach (var control in _pointsGainedControls)
			{
				if (control.Tag is bool direction)
				{
					if (direction)
						control.Y -= 1;
					else
						control.Y += 1;
					control.Alpha = control.Alpha - 10;
					control.Initialize();
					if (control.Alpha <= 0)
						toRemove.Add(control);
				}
				else
					toRemove.Add(control);
			}
			foreach (var remove in toRemove)
			{
				RemoveControl(240, remove);
				_pointsGainedControls.Remove(remove);
			}
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
			_pointsGainedTimer.Update(gameTime.ElapsedGameTime);
		}

		private void TakeTurn()
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			var firstOpponentPoints = Engine.State.FirstOpponentBoard.GetValue();
			var secondOpponentPoints = Engine.State.SecondOpponentBoard.GetValue();

			if (!Engine.TakeTurn())
				return;

			var newFirstOpponentPoints = Engine.State.FirstOpponentBoard.GetValue();
			var newSecondOpponentPoints = Engine.State.SecondOpponentBoard.GetValue();

			_firstOpponentBoard.HideHighlight();
			_secondOpponentBoard.HideHighlight();

			_secondOpponentBoard.UpdateBoard();
			_firstOpponentBoard.UpdateBoard();

			if (firstOpponentPoints != newFirstOpponentPoints)
				_pointsGainedControls.Add(CreatePointsGainedControl(firstOpponentPoints, newFirstOpponentPoints, _firstOpponentPoints.X, _firstOpponentPoints.Y));
			if (secondOpponentPoints != newSecondOpponentPoints)
				_pointsGainedControls.Add(CreatePointsGainedControl(secondOpponentPoints, newSecondOpponentPoints, _secondOpponentPoints.X, _secondOpponentPoints.Y));

			_firstOpponentPoints.Text = $"{Engine.State.FirstOpponentBoard.GetValue()}";
			_secondOpponentPoints.Text = $"{Engine.State.SecondOpponentBoard.GetValue()}";

			if (Engine.GameOver)
			{
				var pointsGained = 0;
				if ((Engine.State.FirstOpponent.MoveModule is PlayerMoveModule) && (Engine.State.SecondOpponent.MoveModule is not PlayerMoveModule) && Engine.State.Winner == Engine.State.FirstOpponent.MoveModule.OpponentID)
				{
					pointsGained = (int)(Engine.State.FirstOpponentBoard.GetValue() * Engine.State.SecondOpponent.Difficulty);
					Parent.User.AppendCompletedItem(Engine.State.SecondOpponent.ID);
					Parent.User.AppendCompletedItem(Engine.State.FirstOpponentBoard.ID);
					Parent.User.AppendCompletedItem(Engine.State.CurrentDice.ID);
				}
				if ((Engine.State.SecondOpponent.MoveModule is PlayerMoveModule) && (Engine.State.FirstOpponent.MoveModule is PlayerMoveModule) && Engine.State.Winner == Engine.State.SecondOpponent.MoveModule.OpponentID)
				{
					pointsGained = (int)(Engine.State.SecondOpponentBoard.GetValue() * Engine.State.FirstOpponent.Difficulty);
					Parent.User.AppendCompletedItem(Engine.State.FirstOpponent.ID);
					Parent.User.AppendCompletedItem(Engine.State.FirstOpponentBoard.ID);
					Parent.User.AppendCompletedItem(Engine.State.CurrentDice.ID);
				}

				Parent.User.Points += pointsGained;
				Parent.User.Save();
				if (pointsGained > 0)
					_pointsGainedLabel.Text = $"Gained {pointsGained} points.";
				else
					_pointsGainedLabel.Text = "No points awarded";

				if (Engine.State.Winner == Engine.State.FirstOpponent.MoveModule.OpponentID)
					_winnerLabel.Text = $"{Engine.State.FirstOpponent.Name} Won!";
				else
					_winnerLabel.Text = $"{Engine.State.SecondOpponent.Name} Won!";
				_gameOverPanel.IsVisible = true;
				if (File.Exists("save.json"))
					File.Delete("save.json");

				return;
			}

			_rolling = true;
		}

		private LabelControl CreatePointsGainedControl(int previous, int now, float sourceX, float sourceY)
		{
			var direction = "";
			if (now > previous)
				direction = "+";

			var control = new LabelControl()
			{
				Text = direction + $"{now - previous}",
				Tag = now > previous,
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				FontColor = now > previous ? Color.LightGreen : Color.DarkRed,
				X = sourceX + 75,
				Y = sourceY
			};
			AddControl(240, control);
			control.Initialize();

			return control;
		}

		private void MoveLeft()
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			Parent.Audio.PlaySoundEffectOnce(new Guid("19268829-42c3-411d-8357-91d55de0cef6"));

			var opponent = Engine.GetCurrentOpponent();
			if (opponent.MoveModule is PlayerMoveModule player && Engine.State.Turn == player.OpponentID)
			{
				var board = Engine.GetCurrentOpponentBoard();
				var current = player.GetTargetColumn();
				current--;
				if (current < 0)
					current = 0;
				player.SetTargetColumn(current);
			}

			if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID)
				_firstOpponentBoard.HighlightColumn(Engine.State.FirstOpponent.MoveModule.GetTargetColumn());
			if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID)
				_secondOpponentBoard.HighlightColumn(Engine.State.SecondOpponent.MoveModule.GetTargetColumn());
		}

		private void MoveRight()
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			Parent.Audio.PlaySoundEffectOnce(new Guid("19268829-42c3-411d-8357-91d55de0cef6"));

			var opponent = Engine.GetCurrentOpponent();
			if (opponent.MoveModule is PlayerMoveModule player && Engine.State.Turn == player.OpponentID)
			{
				var board = Engine.GetCurrentOpponentBoard();
				var current = player.GetTargetColumn();
				current++;
				if (current >= board.Columns.Count)
					current = board.Columns.Count - 1;
				player.SetTargetColumn(current);
			}

			if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID)
				_firstOpponentBoard.HighlightColumn(Engine.State.FirstOpponent.MoveModule.GetTargetColumn());
			if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID)
				_secondOpponentBoard.HighlightColumn(Engine.State.SecondOpponent.MoveModule.GetTargetColumn());
		}

		private void Escape()
		{
			if ((Engine.State.FirstOpponent.MoveModule is PlayerMoveModule) || (Engine.State.SecondOpponent.MoveModule is PlayerMoveModule))
				if (_rolling || _rollWait || _selectWait)
					return;

			if (_rollSoundEffect != Guid.Empty)
				Parent.Audio.StopSoundEffect(_rollSoundEffect);
			ClearLayer(240);
			SwitchView(new MainMenu(Parent));
		}

		private void UpdateColumnHighlight()
		{
			if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID)
			{
				_firstOpponentBoard.HighlightColumn(Engine.State.FirstOpponent.MoveModule.GetTargetColumn());
				_secondOpponentBoard.HideHighlight();
			}
			else if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID)
			{
				_secondOpponentBoard.HighlightColumn(Engine.State.SecondOpponent.MoveModule.GetTargetColumn());
				_firstOpponentBoard.HideHighlight();
			}
		}
	}
}