using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseView
    {
        public static Guid ID = new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0");

        private Guid _selectedBoard = Guid.Empty;
        private ButtonControl? _selectedBoardButton;
        private Guid _selectedFirstOpponent = Guid.Empty;
        private ButtonControl? _selectedFirstOpponentButton;
        private Guid _selectedSecondOpponent = Guid.Empty;
        private ButtonControl? _selectedSecondOpponentButton;
        private Guid _selectedDice = Guid.Empty;
        private ButtonControl? _selectedDiceButton;

        public StartGame(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }

        private void SelectBoard_Click(ButtonControl sender)
        {
            if (_selectedBoardButton == sender)
                return;
            if (sender is ButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.FillColor = BasicTextures.GetBasicRectange(Color.DarkGray);
                    if (_selectedBoardButton != null)
                        _selectedBoardButton.FillColor = BasicTextures.GetBasicRectange(Color.Gray);
                    _selectedBoardButton = sender;
                    _selectedBoard = boardID;
                }
            }
        }

        private void SelectFirstOpponent_Click(ButtonControl sender)
        {
            if (_selectedFirstOpponentButton == sender)
                return;
            if (sender is ButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.FillColor = BasicTextures.GetBasicRectange(Color.DarkGray);
                    if (_selectedFirstOpponentButton != null)
                        _selectedFirstOpponentButton.FillColor = BasicTextures.GetBasicRectange(Color.Gray);
                    _selectedFirstOpponentButton = sender;
                    _selectedFirstOpponent = boardID;
                }
            }
        }

        private void SelectSecondOpponent_Click(ButtonControl sender)
        {
            if (_selectedSecondOpponentButton == sender)
                return;
            if (sender is ButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.FillColor = BasicTextures.GetBasicRectange(Color.DarkGray);
                    if (_selectedSecondOpponentButton != null)
                        _selectedSecondOpponentButton.FillColor = BasicTextures.GetBasicRectange(Color.Gray);
                    _selectedSecondOpponentButton = sender;
                    _selectedSecondOpponent = boardID;
                }
            }
        }

        private void SelectDice_Click(ButtonControl sender)
        {
            if (_selectedDiceButton == sender)
                return;
            if (sender is ButtonControl button)
            {
                if (button.Tag is Guid boardID)
                {
                    sender.FillColor = BasicTextures.GetBasicRectange(Color.DarkGray);
                    if (_selectedDiceButton != null)
                        _selectedDiceButton.FillColor = BasicTextures.GetBasicRectange(Color.Gray);
                    _selectedDiceButton = sender;
                    _selectedDice = boardID;
                }
            }
        }
    }
}
