using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame : BaseNavigatableView
	{
		private Guid _selectedBoard = Guid.Empty;
		private AnimatedAudioButton? _selectedBoardButton;
		private Guid _selectedFirstOpponent = Guid.Empty;
		private AnimatedAudioButton? _selectedFirstOpponentButton;
		private Guid _selectedSecondOpponent = Guid.Empty;
		private AnimatedAudioButton? _selectedSecondOpponentButton;
		private Guid _selectedDice = Guid.Empty;
		private AnimatedAudioButton? _selectedDiceButton;

		public StartGame(KnuckleBoneWindow parent) : base(parent, new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0"), new List<int>() { 0, 1 })
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			AcceptAction = () => Start();
			Initialize();
		}

		private void Start()
		{
			if (_selectedBoard == Guid.Empty)
				return;
			if (_selectedFirstOpponent == Guid.Empty)
				return;
			if (_selectedSecondOpponent == Guid.Empty)
				return;
			if (_selectedDice == Guid.Empty)
				return;

			var state = new GameState(
				ResourceManager.Opponents.GetResource(_selectedFirstOpponent).Clone(),
				ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
				ResourceManager.Opponents.GetResource(_selectedSecondOpponent).Clone(),
				ResourceManager.Boards.GetResource(_selectedBoard).Clone(),
				ResourceManager.Dice.GetResource(_selectedDice).Clone(),
				Parent.User.Clone()
				);

			state.FirstOpponent.MoveModule.OpponentID = Guid.NewGuid();
			state.SecondOpponent.MoveModule.OpponentID = Guid.NewGuid();
			state.SetRandomStartingPlayer();

			GameSaveHelpers.Save(state);

			SwitchView(new MainGame(Parent, state));
		}

		private void SelectBoard_Click(AnimatedAudioButton sender)
		{
			if (_selectedBoardButton == sender)
				return;
			if (sender is AnimatedAudioButton button)
			{
				if (button.Tag is IDefinition def)
				{
					sender.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
					if (_selectedBoardButton != null)
						_selectedBoardButton.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
					_selectedBoardButton = sender;
					_selectedBoard = def.ID;
					_boardsDescription.Text = def.Description;
				}
			}
		}

		private void SelectFirstOpponent_Click(AnimatedAudioButton sender)
		{
			if (_selectedFirstOpponentButton == sender)
				return;
			if (sender is AnimatedAudioButton button)
			{
				if (button.Tag is OpponentDefinition def)
				{
					sender.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
					if (_selectedFirstOpponentButton != null)
						_selectedFirstOpponentButton.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
					_selectedFirstOpponentButton = sender;
					_selectedFirstOpponent = def.ID;

					var sb = new StringBuilder();
					sb.AppendLine(def.Description);
					if (def.Difficulty != 1)
						sb.AppendLine($"Score Multiplier: {def.Difficulty}");
					_firstOpponentDescription.Text = sb.ToString();
				}
			}
		}

		private void SelectSecondOpponent_Click(AnimatedAudioButton sender)
		{
			if (_selectedSecondOpponentButton == sender)
				return;
			if (sender is AnimatedAudioButton button)
			{
				if (button.Tag is OpponentDefinition def)
				{
					sender.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
					if (_selectedSecondOpponentButton != null)
						_selectedSecondOpponentButton.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
					_selectedSecondOpponentButton = sender;
					_selectedSecondOpponent = def.ID;

					var sb = new StringBuilder();
					sb.AppendLine(def.Description);
					if (def.Difficulty != 1)
						sb.AppendLine($"Score Multiplier: {def.Difficulty}");
					_secondOpponentDescription.Text = sb.ToString();
				}
			}
		}

		private void SelectDice_Click(AnimatedAudioButton sender)
		{
			if (_selectedDiceButton == sender)
				return;
			if (sender is AnimatedAudioButton button)
			{
				if (button.Tag is IDefinition def)
				{
					sender.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
					if (_selectedDiceButton != null)
						_selectedDiceButton.TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
					_selectedDiceButton = sender;
					_selectedDice = def.ID;
					_diceDescription.Text = def.Description;
				}
			}
		}
	}
}
