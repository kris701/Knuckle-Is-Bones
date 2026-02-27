using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Engines.Actions;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Game.MoveModules;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public partial class MainGame : BaseNavigatableView
	{
		public IKnuckleBonesEngine Engine { get; set; }

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
		private readonly GamepadRumbler _hitRumbler;

		private int _currentFirstOpponentPoints = 0;
		private int _currentSecondOpponentPoints = 0;

		private readonly List<LabelControl> _pointsGainedControls = new List<LabelControl>();

		public MainGame(KnuckleBoneWindow parent, GameState state) : base(parent, new Guid("d5b46cf0-03bd-4226-a765-b00f39fdf361"), new List<int>() { 10, 20, 1001 })
		{
			Engine = new KnuckleBonesEngine(state);
			Engine.OnOpponentDiceRemoved += () => {
				Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.GameOnDiceRemove);
				if (InputType == InputTypes.Gamepad)
					_hitRumbler!.Rumble();
			};
			Engine.OnCombo += () => Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.GameOnCombo);
			Engine.OnTurn += () => Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.GameOnTurn);
			Engine.OnBoardModified += (o) => Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.GameOnBoardModified);

			var speeds = GameSpeedHelpers.GetGameSpeed(Parent.Settings.GameSpeed);
			_rollTimer = new GameTimer(TimeSpan.FromMilliseconds(speeds.RollTimer), OnRollTimer);
			_rollWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(speeds.RollWaitTimer), OnRollWaitTimer);
			_modifyWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(speeds.ModifyWaitTimer), OnModifyWaitTimer);
			_selectWaitTimer = new GameTimer(TimeSpan.FromMilliseconds(speeds.SelectWaitTimer), OnSelectWaitTimer);
			_pointsGainedTimer = new GameTimer(TimeSpan.FromMilliseconds(speeds.PointsGainedTimer), OnPointsGainedTimer);
			
			_hitRumbler = new GamepadRumbler(new List<int>() { 0, 1, 2, 3 }, TimeSpan.FromMilliseconds(100));

			BackAction = () => Escape();

			var cap = GamePad.GetCapabilities(1);
			var tst = GamePad.SetVibration(0, 0, 0);

			Initialize();
		}

		private void OnRollTimer(TimeSpan span)
		{
			if (_rollSoundEffect == Guid.Empty)
				_rollSoundEffect = Parent.Audio.PlaySoundEffect(SoundEffectHelpers.GameOnDiceRoll);
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
				_diceLabel.Text = $"{Engine.State.CurrentDice.RollValueIndependent()}";
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

			var current = Engine.State.GetCurrentOpponent();

			if (current.MoveModule.OpponentID == Engine.State.FirstOpponent.MoveModule.OpponentID)
				_firstOpponentTurnControl.FontColor = FontHelpers.SecondaryColor;
			else
				_secondOpponentTurnControl.FontColor = FontHelpers.SecondaryColor;

			if (current.MoveModule is not PlayerMoveModule)
			{
				_firstOpponentBoard.HideHighlight();
				_secondOpponentBoard.HideHighlight();
				_shouldModifyWait = false;
				_modifyWait = false;
				_selectWait = false;
				if (current.MoveModule is IBoardModifier)
					_shouldModifyWait = Engine.Execute(new SetCPUBoardModificationAction());
				if (!Engine.Execute(new CheckGameStateAction()))
					Engine.Execute(new SetCPUMoveAction());
				if (_shouldModifyWait)
				{
					_secondOpponentBoard!.UpdateBoard();
					_firstOpponentBoard!.UpdateBoard();
					if (current.MoveModule.OpponentID == Engine.State.FirstOpponent.MoveModule.OpponentID)
						_firstOpponentBoard.ShowModifying();
					else
						_secondOpponentBoard.ShowModifying();
					_modifyWait = true;
				}
				else
				{
					UpdateColumnHighlight();
					_selectWait = true;
				}
			}
			else
			{
				UpdateForMove();
				if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID && Engine.State.FirstOpponent.MoveModule is PlayerMoveModule)
					_firstOpponentBoard.CanSelect = true;
				if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID && Engine.State.SecondOpponent.MoveModule is PlayerMoveModule)
					_secondOpponentBoard.CanSelect = true;

				if (InputType == InputTypes.Keyboard)
				{
					_keyboardNavigator.Selector.IsVisible = true;
				}
				if (InputType == InputTypes.Gamepad)
				{
					_gamepadNavigator.Selector.IsVisible = true;
				}
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
				_rollTimer.Update(gameTime);
			if (_rollWait)
				_rollWaitTimer.Update(gameTime);
			if (_selectWait)
				_selectWaitTimer.Update(gameTime);
			if (_modifyWait)
				_modifyWaitTimer.Update(gameTime);
			_pointsGainedTimer.Update(gameTime);
			_hitRumbler.Update(gameTime);
			base.OnUpdate(gameTime);
		}

		private void TakeTurn()
		{
			if (_rolling || _rollWait || _selectWait)
				return;

			_firstOpponentTurnControl.FontColor = FontHelpers.PrimaryColor;
			_secondOpponentTurnControl.FontColor = FontHelpers.PrimaryColor;

			HideAllNavigators();

			Engine.Execute(new TurnAction());

			var newFirstOpponentPoints = Engine.State.GetFirstOpponentBoardValue();
			var newSecondOpponentPoints = Engine.State.GetSecondOpponentBoardValue();

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

			if (Engine.State.GameOver)
			{
				_keyboardNavigator.SelectorLocation = NavigatorSelectorLocations.Left;
				_gamepadNavigator.SelectorLocation = NavigatorSelectorLocations.Left;

				var result = Engine.State.GetGameResult();

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
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
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

			Parent.Audio.PlaySoundEffectOnce(SoundEffectHelpers.GameOnMove);

			var opponent = Engine.State.GetCurrentOpponent();
			if (opponent.MoveModule is PlayerMoveModule player && Engine.State.Turn == player.OpponentID)
			{
				if (to < 0)
					to = 0;
				if (to >= Engine.State.FirstOpponentBoard.Columns.Count)
					to = Engine.State.FirstOpponentBoard.Columns.Count - 1;
				Engine.Execute(new SetPlayerMoveAction(to));
			}

			_keyboardNavigator.Selector.IsVisible = false;
			_gamepadNavigator.Selector.IsVisible = false;

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
				if (Engine.State.FirstOpponent.MoveModule.TargetColumn != -1)
					_firstOpponentBoard.HighlightColumn(Engine.State.FirstOpponent.MoveModule.TargetColumn);
				_secondOpponentBoard.HideHighlight();
			}
			else if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID)
			{
				if (Engine.State.SecondOpponent.MoveModule.TargetColumn != -1)
					_secondOpponentBoard.HighlightColumn(Engine.State.SecondOpponent.MoveModule.TargetColumn);
				_firstOpponentBoard.HideHighlight();
			}
		}

		private void HideAllNavigators()
		{
			_secondOpponentBoard.DisableAll();
			_firstOpponentBoard.DisableAll();
		}

		private void UpdateForMove()
		{
			if (Engine.State.Turn == Engine.State.FirstOpponent.MoveModule.OpponentID)
			{
				_keyboardNavigator.SelectorLocation = FormMatter.OpenGL.Input.NavigatorSelectorLocations.Bottom;
				_gamepadNavigator.SelectorLocation = FormMatter.OpenGL.Input.NavigatorSelectorLocations.Bottom;
				_firstOpponentBoard.EnableAll();

				if (InputType == InputTypes.Keyboard)
					_keyboardNavigator.GotoClosest(this);
				if (InputType == InputTypes.Gamepad)
					_gamepadNavigator.GotoClosest(this);
			}
			else if (Engine.State.Turn == Engine.State.SecondOpponent.MoveModule.OpponentID)
			{
				_keyboardNavigator.SelectorLocation = FormMatter.OpenGL.Input.NavigatorSelectorLocations.Top;
				_gamepadNavigator.SelectorLocation = FormMatter.OpenGL.Input.NavigatorSelectorLocations.Top;
				_secondOpponentBoard.EnableAll();

				if (InputType == InputTypes.Keyboard)
					_keyboardNavigator.GotoClosest(this);
				if (InputType == InputTypes.Gamepad)
					_gamepadNavigator.GotoClosest(this);
			}
		}
	}
}