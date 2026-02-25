using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public partial class MainGame : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361");

		public KnuckleBonesEngine Engine { get; set; }

		private readonly GameTimer _rollTimer;
		private readonly GameTimer _rollWaitTimer;
		private readonly GameTimer _selectWaitTimer;
		private readonly GameTimer _modifyWaitTimer;
		private readonly GameTimer _pointsGainedTimer;
		private bool _selectWait = false;
		private bool _modifyWait = false;
		private bool _shouldModifyWait = false;
		private bool _rolling = true;
		private bool _rollWait = false;
		private bool _exiting = false;
		private int _rolledTimes = 0;
		private readonly Random _rnd = new Random();
		private Guid _rollSoundEffect = Guid.Empty;

		private int _currentFirstOpponentPoints = 0;
		private int _currentSecondOpponentPoints = 0;

		private readonly List<LabelControl> _pointsGainedControls = new List<LabelControl>();

		public MainGame(KnuckleBoneWindow parent, GameState state) : base(parent, ID)
		{
			Engine = new KnuckleBonesEngine(state);
			Engine.OnOpponentDiceRemoved += () => Parent.Audio.PlaySoundEffectOnce(new Guid("4e53cd32-7af6-47a1-a331-ec2096505c78"));
			Engine.OnCombo += () => Parent.Audio.PlaySoundEffectOnce(new Guid("74ea48c8-cb6f-4a22-8226-e5d6142b1f76"));
			Engine.OnTurn += () => Parent.Audio.PlaySoundEffectOnce(new Guid("23ac297f-3e68-461f-a869-a304e89e18c6"));
			Engine.OnBoardModified += (o) =>
			{
				_secondOpponentBoard!.UpdateBoard();
				_firstOpponentBoard!.UpdateBoard();
				if (o == Engine.State.FirstOpponent.MoveModule.OpponentID)
					_firstOpponentBoard.ShowModifying(); 
				else
					_secondOpponentBoard.ShowModifying();

				_shouldModifyWait = true;
				Parent.Audio.PlaySoundEffectOnce(new Guid("97b1fabe-d7c8-44fc-86bf-94592a91edf8"));
			};

			_rollTimer = new GameTimer(TimeSpan.FromMilliseconds(150), OnRollTimer);
			_rollWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(500), OnRollWaitTimer);
			_modifyWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(1000), OnModifyWaitTimer);
			_selectWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(1000), OnSelectWaitTimer);
			_pointsGainedTimer = new GameTimer(TimeSpan.FromMilliseconds(100), OnPointsGainedTimer);
			Initialize();
		}

		private void OnRollTimer(TimeSpan span)
		{
			if (_rollSoundEffect == Guid.Empty)
				_rollSoundEffect = Parent.Audio.PlaySoundEffect(new Guid("adb4826c-ae62-4785-b0f3-81dd4d692920"));
			_rolledTimes++;
			if (_rolledTimes > 10)
			{
				_rolledTimes = 0;
				_rolling = false;
				_rollWait = true;
				Parent.Audio.StopSoundEffect(_rollSoundEffect);
				_rollSoundEffect = Guid.Empty;
				_diceLabel.Text = $"{Engine.State.CurrentDice.Value}";
				_diceLabel.X = 375;
				_diceLabel.Y = 475;
				_diceLabel.Rotation = 0;
				_diceLabel.Initialize();
			}
			else
			{
				_diceLabel.Text = $"{_rnd.Next(1, Engine.State.CurrentDice.Sides + 1)}";
				_diceLabel.X = 375 + _rnd.Next(-50, 50);
				_diceLabel.Y = 475 + _rnd.Next(-50, 50);
				_diceLabel.Rotation = (float)(3.14 * _rnd.NextDouble());
				_diceLabel.Initialize();
			}
		}

		private void OnRollWaitTimer(TimeSpan span)
		{
			_rollWait = false;
			_diceLabel.Text = $"{Engine.State.CurrentDice.Value}";

			var current = Engine.GetCurrentOpponent();
			if (current.MoveModule is not PlayerMoveModule)
			{
				_firstOpponentBoard.HideHighlight();
				_secondOpponentBoard.HideHighlight();
				_shouldModifyWait = false;
				_modifyWait = false;
				_selectWait = false;
				Engine.SetCPUOpponentsMove();
				if (_shouldModifyWait)
					_modifyWait = true;
				else
				{
					UpdateColumnHighlight();
					_selectWait = true;
				}
			}
			else
			{
				if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID && Engine.State.FirstOpponent.MoveModule is PlayerMoveModule)
					_firstOpponentBoard.CanSelect = true;
				if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID && Engine.State.SecondOpponent.MoveModule is PlayerMoveModule)
					_secondOpponentBoard.CanSelect = true;
			}
		}

		private void OnSelectWaitTimer(TimeSpan span)
		{
			_firstOpponentBoard.HideHighlight();
			_secondOpponentBoard.HideHighlight();
			_selectWait = false;
			TakeTurn();
		}

		private void OnModifyWaitTimer(TimeSpan span)
		{
			_modifyWait = false;
			_selectWait = true;
			UpdateColumnHighlight();
		}

		private void OnPointsGainedTimer(TimeSpan span)
		{
			var toRemove = new List<LabelControl>();
			foreach (var control in _pointsGainedControls)
			{
				if (!control.IsInitialized)
				{
					AddControl(240, control);
					control.Initialize();
				}
				if (_exiting)
					return;
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
				if (_exiting)
					return;
				RemoveControl(240, remove);
				_pointsGainedControls.Remove(remove);
			}
		}

		public override void OnUpdate(GameTime gameTime)
		{
			if (_exiting)
				return;

			if (_rolling)
				_rollTimer.Update(gameTime.ElapsedGameTime);
			if (_rollWait)
				_rollWaitTimer.Update(gameTime.ElapsedGameTime);
			if (_selectWait)
				_selectWaitTimer.Update(gameTime.ElapsedGameTime);
			if (_modifyWait)
				_modifyWaitTimer.Update(gameTime.ElapsedGameTime);
			_pointsGainedTimer.Update(gameTime.ElapsedGameTime);
		}

		private void TakeTurn()
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			if (!Engine.TakeTurn())
				return;

			var newFirstOpponentPoints = Engine.GetFirstOpponentBoardValue();
			var newSecondOpponentPoints = Engine.GetSecondOpponentBoardValue();

			_secondOpponentBoard.UpdateBoard();
			_firstOpponentBoard.UpdateBoard();

			if (_currentFirstOpponentPoints != newFirstOpponentPoints)
				_pointsGainedControls.Add(CreatePointsGainedControl(_currentFirstOpponentPoints, newFirstOpponentPoints, _firstOpponentPoints.X, _firstOpponentPoints.Y));
			if (_currentSecondOpponentPoints != newSecondOpponentPoints)
				_pointsGainedControls.Add(CreatePointsGainedControl(_currentSecondOpponentPoints, newSecondOpponentPoints, _secondOpponentPoints.X, _secondOpponentPoints.Y));

			_currentFirstOpponentPoints = newFirstOpponentPoints;
			_currentSecondOpponentPoints = newSecondOpponentPoints;

			_firstOpponentPoints.Text = $"{newFirstOpponentPoints}";
			_secondOpponentPoints.Text = $"{newSecondOpponentPoints}";

			if (Engine.GameOver)
			{
				var result = Engine.GetGameResult();

				foreach (var completedItem in result.CompletedItems)
					Parent.User.AppendCompletedItem(completedItem);
				Parent.User.Points += result.PointsGained;
				UserSaveHelpers.Save(Parent.User);

				if (result.PointsGained > 0)
					_pointsGainedLabel.Text = $"Gained {result.PointsGained} points.";
				else
					_pointsGainedLabel.Text = "No points awarded";

				if (result.HadPlayer)
				{
					if (result.PlayerWon)
						_winnerLabel.Text = $"You Won!";
					else
						_winnerLabel.Text = $"You lost to {result.WinnerName}!";
				}
				else
					_winnerLabel.Text = $"{result.WinnerName} Won!";

				GameSaveHelpers.DeleteSave();

				AchievementHelper.UpdateAchievements(Parent.User);

				_gameOverPanel.IsVisible = true;
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

			return control;
		}

		private void MoveSet(int to)
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			Parent.Audio.PlaySoundEffectOnce(new Guid("19268829-42c3-411d-8357-91d55de0cef6"));

			var opponent = Engine.GetCurrentOpponent();
			if (opponent.MoveModule is PlayerMoveModule player && Engine.State.Turn == player.OpponentID)
			{
				if (to < 0)
					to = 0;
				if (to >= Engine.State.FirstOpponentBoard.Columns.Count)
					to = Engine.State.FirstOpponentBoard.Columns.Count - 1;
				player.SetTargetColumn(to);
			}

			_firstOpponentBoard.CanSelect = false;
			_secondOpponentBoard.CanSelect = false;
			_firstOpponentBoard.HideHighlight();
			_secondOpponentBoard.HideHighlight();

			TakeTurn();
		}

		private void Escape()
		{
			if ((Engine.State.FirstOpponent.MoveModule is PlayerMoveModule) || (Engine.State.SecondOpponent.MoveModule is PlayerMoveModule))
				if (_rolling || _rollWait || _selectWait)
					return;

			_exiting = true;

			if (_rollSoundEffect != Guid.Empty)
				Parent.Audio.StopSoundEffect(_rollSoundEffect);
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