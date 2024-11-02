using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseAnimatedView
    {
        public static Guid ID = new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0");

        private Guid _selectedBoard = Guid.Empty;
        private AnimatedButtonControl? _selectedBoardButton;
        private Guid _selectedFirstOpponent = Guid.Empty;
        private AnimatedButtonControl? _selectedFirstOpponentButton;
        private Guid _selectedSecondOpponent = Guid.Empty;
        private AnimatedButtonControl? _selectedSecondOpponentButton;
        private Guid _selectedDice = Guid.Empty;
        private AnimatedButtonControl? _selectedDiceButton;

        public StartGame(IWindow parent) : base(
            parent,
            ID,
            parent.Textures.GetTextureSet(new Guid("aeb7a8dd-d580-4550-af87-1db80e522dfa")),
            parent.Textures.GetTextureSet(new Guid("e650684b-a4cf-4950-990e-60b08aac57c5")))
        {
            Initialize();
        }

        private void SelectBoard_Click(AnimatedButtonControl sender)
        {
            if (_selectedBoardButton == sender)
                return;
            if (sender is AnimatedButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedBoardButton != null)
                        _selectedBoardButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedBoardButton = sender;
                    _selectedBoard = boardID;
                }
            }
        }

        private void SelectFirstOpponent_Click(AnimatedButtonControl sender)
        {
            if (_selectedFirstOpponentButton == sender)
                return;
            if (sender is AnimatedButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedFirstOpponentButton != null)
                        _selectedFirstOpponentButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedFirstOpponentButton = sender;
                    _selectedFirstOpponent = boardID;
                }
            }
        }

        private void SelectSecondOpponent_Click(AnimatedButtonControl sender)
        {
            if (_selectedSecondOpponentButton == sender)
                return;
            if (sender is AnimatedButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedSecondOpponentButton != null)
                        _selectedSecondOpponentButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedSecondOpponentButton = sender;
                    _selectedSecondOpponent = boardID;
                }
            }
        }

        private void SelectDice_Click(AnimatedButtonControl sender)
        {
            if (_selectedDiceButton == sender)
                return;
            if (sender is AnimatedButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
                    if (_selectedDiceButton != null)
                        _selectedDiceButton.TileSet = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
                    _selectedDiceButton = sender;
                    _selectedDice = boardID;
                }
            }
        }
    }
}
