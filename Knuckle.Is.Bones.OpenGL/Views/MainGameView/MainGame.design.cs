using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.Core.Models.Game.OpponentModules;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Knuckle.Is.Bones.OpenGL.Views.StartGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseAnimatedView
    {
        private LabelControl _diceLabel;
        private BoardControl _board1;
        private BoardControl _board2;
        private StackPanelControl _gameOverPanel;
        private LabelControl _winnerLabel;

        public override void Initialize()
        {
            AddControl(0, new TileControl()
            {
                Width = 1920,
                Height = 1080,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            AddControl(0, new LabelControl()
            {
                Text = $"{Engine.State.FirstOpponent.Name}",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Width = 300,
                Height = 100,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
            });
            AddControl(0, new LabelControl()
            {
                Text = $"{Engine.State.SecondOpponent.Name}",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Width = 300,
                Height = 100,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
            });

            if (Engine.State.FirstOpponent.Module is PlayerOpponentModule || Engine.State.SecondOpponent.Module is PlayerOpponentModule)
                SetupControlsView();

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

            UpdateFirstOpponentBoard();
            UpdateSecondOpponentBoard();
        }

        private void SetupControlsView()
        {
            AddControl(0, new AnimatedLabelControl()
            {
                X = 1500,
                Y = 900,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
                FontColor = Color.White,
                Text = $"<",
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"))
            });
            AddControl(0, new AnimatedLabelControl()
            {
                X = 1700,
                Y = 900,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
                FontColor = Color.White,
                Text = $">",
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"))
            });
            AddControl(0, new AnimatedLabelControl()
            {
                X = 1300,
                Y = 900,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                FontColor = Color.White,
                Text = $"Enter",
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"))
            });
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
            AddControl(layer, new AnimatedLabelControl()
            {
                X = x,
                Y = y,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                FontColor = Color.Gold,
                Text = $"{board.GetValue()}",
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"))
            });
        }

        private void SetupDice()
        {
            _diceLabel = new AnimatedLabelControl()
            {
                X = 375,
                VerticalAlignment = VerticalAlignment.Middle,
                Width = 150,
                Height = 150,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = "",
                FontColor = Color.White,
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"))
            };
            AddControl(1000, _diceLabel);
        }

        private void SetupGameOverView()
        {
            _winnerLabel = new LabelControl()
            {
                Width = 500,
                Height = 100,
                HorizontalAlignment = HorizontalAlignment.Middle,
                Y = 100,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                Text = "",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            };
            _gameOverPanel = new StackPanelControl(new List<IControl>()
            {
                new AnimatedTileControl()
                {
                    Width = 600,
                    Height = 500,
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d7ae88e1-8b8e-4ea9-9c99-e78e2d91943a"))
                },
                new LabelControl()
                {
                    Y = 20,
                    Width = 500,
                    Height = 100,
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    Text = $"Game Over!",
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Black)
                },
                _winnerLabel,
                new AnimatedButtonControl(Parent, (x) => SwitchView(new StartGame(Parent)))
                {
                    Width = 500,
                    Height = 110,
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    Y = 230,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = "Play Again",
                    FontColor = Color.White,
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray),
                },
                new AnimatedButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
                {
                    Width = 500,
                    Height = 110,
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    Y = 350,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = "Main Menu",
                    FontColor = Color.White,
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray),
                }
            })
            {
                HorizontalAlignment = HorizontalAlignment.Middle,
                VerticalAlignment = VerticalAlignment.Middle,
                Width = 600,
                Height = 500,
                IsVisible = false
            };
            AddControl(1001, _gameOverPanel);
        }
    }
}