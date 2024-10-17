using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Knuckle.Is.Bones.OpenGL.Views.StartGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        private LabelControl _diceLabel;
        private BoardControl _board1;
        private BoardControl _board2;
        private StackPanelControl _gameOverPanel;
        private LabelControl _winnerLabel;

        public override void Initialize()
        {
            AddControl(0, new LabelControl()
            {
                Text = $"{Engine.State.FirstOpponent.Name}",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Width = 200,
                Height = 520,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });
            AddControl(0, new LabelControl()
            {
                Text = $"{Engine.State.SecondOpponent.Name}",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Width = 200,
                Height = 520,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });
            AddControl(0, new TileControl()
            {
                Width = 1920,
                Height = 40,
                VerticalAlignment = VerticalAlignment.Middle,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            UpdateFirstOpponentBoard();
            UpdateSecondOpponentBoard();
            SetupDice();
            SetupGameOverView();

#if DEBUG
            AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new MainGame(Parent, Save)))
            {
                X = 0,
                Y = 0,
                Width = 50,
                Height = 25,
                Text = "Reload",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx10),
                FillColor = BasicTextures.GetBasicRectange(Color.White),
                FontColor = Color.Black,
                FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
#endif

            base.Initialize();
        }

        private void UpdateFirstOpponentBoard()
        {
            ClearLayer(10);
            _board1 = new BoardControl(Parent, Engine.State.FirstOpponentBoard, 710, 10, 500, 500, true);
            GeneratePointsBox(10, Engine.State.FirstOpponentBoard, 1390, 175);
            if (!_rolling && Engine.State.Turn == Engine.State.FirstOpponent.Module.OpponentID)
                _board1.HighlightColumn(Engine.State.FirstOpponent.Module.GetTargetColumn());
            AddControl(10, _board1);
        }

        private void UpdateSecondOpponentBoard()
        {
            ClearLayer(20);
            _board2 = new BoardControl(Parent, Engine.State.SecondOpponentBoard, 710, 570, 500, 500, false);
            GeneratePointsBox(20, Engine.State.SecondOpponentBoard, 375, 735);
            if (!_rolling && Engine.State.Turn == Engine.State.SecondOpponent.Module.OpponentID)
                _board2.HighlightColumn(Engine.State.SecondOpponent.Module.GetTargetColumn());
            AddControl(20, _board2);
        }

        private void GeneratePointsBox(int layer, BoardDefinition board, int x, int y)
        {
            AddControl(layer, new LabelControl()
            {
                X = x,
                Y = y,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
                FontColor = Color.Black,
                Text = $"{board.GetValue()}",
                FillColor = BasicTextures.GetBasicRectange(Color.Gold)
            });
        }

        private void SetupDice()
        {
            _diceLabel = new LabelControl()
            {
                X = 375,
                VerticalAlignment = VerticalAlignment.Middle,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
                Text = "",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Blue)
            };
            AddControl(1000, _diceLabel);
        }

        private void SetupGameOverView()
        {
            _winnerLabel = new LabelControl()
            {
                Width = 400,
                Height = 100,
                Y = 100,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                Text = "",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            };
            _gameOverPanel = new StackPanelControl(new List<IControl>()
            {
                new LabelControl()
                {
                    Width = 400,
                    Height = 100,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    Text = $"Game Over!",
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Black)
                },
                _winnerLabel,
                new ButtonControl(Parent, (x) => SwitchView(new StartGame(Parent)))
                {
                    Width = 400,
                    Height = 110,
                    Y = 200,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = "Play Again",
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Black),
                    FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray),
                },
                new ButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
                {
                    Width = 400,
                    Height = 110,
                    Y = 300,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = "Main Menu",
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Black),
                    FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray),
                }
            })
            {
                HorizontalAlignment = HorizontalAlignment.Middle,
                VerticalAlignment = VerticalAlignment.Middle,
                Width = 400,
                Height = 300,
                IsVisible = false
            };
            AddControl(1001, _gameOverPanel);
        }
    }
}