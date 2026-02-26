using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Input;
using System;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame : BaseKnuckleBoneFadeView
	{
		private Guid _selectedBoard = Guid.Empty;
		private AnimatedAudioButton? _selectedBoardButton;
		private Guid _selectedFirstOpponent = Guid.Empty;
		private AnimatedAudioButton? _selectedFirstOpponentButton;
		private Guid _selectedSecondOpponent = Guid.Empty;
		private AnimatedAudioButton? _selectedSecondOpponentButton;
		private Guid _selectedDice = Guid.Empty;
		private AnimatedAudioButton? _selectedDiceButton;
		private readonly KeyWatcher _escapeKeyWatcher;

		public StartGame(KnuckleBoneWindow parent) : base(parent, new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0"))
		{
			_escapeKeyWatcher = new KeyWatcher(Keys.Escape, () => SwitchView(new MainMenu(parent)));
			Initialize();
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

		public override void OnUpdate(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();
			_escapeKeyWatcher.Update(keyState);
		}
	}
}
