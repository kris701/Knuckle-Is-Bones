using Knuckle.Is.Bones.Core.Engines.Game;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame : BaseNavigatableView
	{
		private readonly LastGameSetupModel.LastGameSetupType _type;

		private Guid? _selectedBoard = null;
		private Guid? _selectedDice = null;
		private Guid? _selectedFirstOpponent = null;
		private Guid? _selectedSecondOpponent = null;

		public StartGame(KnuckleBoneWindow parent, LastGameSetupModel.LastGameSetupType type) : base(parent, new Guid("b350e448-f201-46ce-baee-1df03f1dbf5c"), new List<int>() { 0 })
		{
			_gamepadNavigator.SelectorOffsetX = -30;
			_keyboardNavigator.SelectorOffsetX = -30;

			BackAction = () => SwitchView(new GametypeSelect(Parent));
			AcceptAction = () => Start();
			_type = type;
			Initialize();

			switch (_type)
			{
				case LastGameSetupModel.LastGameSetupType.PvP:
					_selectedFirstOpponent = new Guid("d6032478-b6ec-483e-8750-5976830d66b2");
					_selectedSecondOpponent = new Guid("d6032478-b6ec-483e-8750-5976830d66b2");

					if (parent.User.LastGameSetup.ContainsKey(LastGameSetupModel.LastGameSetupType.PvP))
					{
						var last = parent.User.LastGameSetup[LastGameSetupModel.LastGameSetupType.PvP];
						_boardSelector?.SetByID(last.BoardID);
						_diceSelector?.SetByID(last.DiceID);
					}
					break;
				case LastGameSetupModel.LastGameSetupType.PvE:
					_selectedFirstOpponent = new Guid("d6032478-b6ec-483e-8750-5976830d66b2");

					if (parent.User.LastGameSetup.ContainsKey(LastGameSetupModel.LastGameSetupType.PvE))
					{
						var last = parent.User.LastGameSetup[LastGameSetupModel.LastGameSetupType.PvE];
						_boardSelector?.SetByID(last.BoardID);
						_diceSelector?.SetByID(last.DiceID);
						_secondOpponentSelector?.SetByID(last.SecondOpponentID);
					}
					break;
				case LastGameSetupModel.LastGameSetupType.EvE:
					if (parent.User.LastGameSetup.ContainsKey(LastGameSetupModel.LastGameSetupType.EvE))
					{
						var last = parent.User.LastGameSetup[LastGameSetupModel.LastGameSetupType.EvE];
						_boardSelector?.SetByID(last.BoardID);
						_diceSelector?.SetByID(last.DiceID);
						_firstOpponentSelector?.SetByID(last.FirstOpponentID);
						_secondOpponentSelector?.SetByID(last.SecondOpponentID);
					}
					break;
			}
		}

		private void Start()
		{
			var boardId = _selectedBoard;
			if (boardId == null)
				boardId = _boardSelector?.CurrentDefinition.ID;
			var diceId = _selectedDice;
			if (diceId == null)
				diceId = _diceSelector?.CurrentDefinition.ID;
			var firstOpponentId = _selectedFirstOpponent;
			if (firstOpponentId == null)
				firstOpponentId = _firstOpponentSelector?.CurrentDefinition.ID;
			var secondOpponentId = _selectedSecondOpponent;
			if (secondOpponentId == null)
				secondOpponentId = _secondOpponentSelector?.CurrentDefinition.ID;

			if (boardId == null || diceId == null || firstOpponentId == null || secondOpponentId == null)
				return;

			Parent.User.LastGameSetup[_type] = new LastGameSetupModel()
			{
				BoardID = (Guid)boardId,
				DiceID = (Guid)diceId,
				FirstOpponentID = (Guid)firstOpponentId,
				SecondOpponentID = (Guid)secondOpponentId
			};
			UserSaveHelpers.Save(Parent.User);

			var state = new GameState(
				ResourceManager.Opponents.GetResource((Guid)firstOpponentId).Clone(),
				ResourceManager.Boards.GetResource((Guid)boardId).Clone(),
				ResourceManager.Opponents.GetResource((Guid)secondOpponentId).Clone(),
				ResourceManager.Boards.GetResource((Guid)boardId).Clone(),
				ResourceManager.Dice.GetResource((Guid)diceId).Clone(),
				Parent.User.Clone()
				);

			state.FirstOpponent.MoveModule.OpponentID = Guid.NewGuid();
			state.SecondOpponent.MoveModule.OpponentID = Guid.NewGuid();
			state.SetRandomStartingPlayer();

			GameSaveHelpers.Save(state);

			SwitchView(new MainGame(Parent, state));
		}
	}
}
