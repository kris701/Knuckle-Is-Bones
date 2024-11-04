using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.OpenGL.Controls;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseKnuckleBoneFadeView
    {
        public static Guid ID = new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0");

        private Guid _selectedBoard = Guid.Empty;
        private AnimatedAudioButton? _selectedBoardButton;
        private Guid _selectedFirstOpponent = Guid.Empty;
        private AnimatedAudioButton? _selectedFirstOpponentButton;
        private Guid _selectedSecondOpponent = Guid.Empty;
        private AnimatedAudioButton? _selectedSecondOpponentButton;
        private Guid _selectedDice = Guid.Empty;
        private AnimatedAudioButton? _selectedDiceButton;

        public StartGame(KnuckleBoneWindow parent) : base(parent, ID)
        {
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
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedBoardButton != null)
                        _selectedBoardButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
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
                if (button.Tag is IDefinition def)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedFirstOpponentButton != null)
                        _selectedFirstOpponentButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedFirstOpponentButton = sender;
                    _selectedFirstOpponent = def.ID;
                    _firstOpponentDescription.Text = def.Description;
                }
            }
        }

        private void SelectSecondOpponent_Click(AnimatedAudioButton sender)
        {
            if (_selectedSecondOpponentButton == sender)
                return;
            if (sender is AnimatedAudioButton button)
            {
                if (button.Tag is IDefinition def)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedSecondOpponentButton != null)
                        _selectedSecondOpponentButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedSecondOpponentButton = sender;
                    _selectedSecondOpponent = def.ID;
                    _secondOpponentDescription.Text = def.Description;
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
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedDiceButton != null)
                        _selectedDiceButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedDiceButton = sender;
                    _selectedDice = def.ID;
                    _diceDescription.Text = def.Description;
                }
            }
        }
    }
}
