using KnuckleBones.Core.Models.Game;
using KnuckleBones.Core.Models.Game.Opponents;
using KnuckleBones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System.Collections.Generic;

namespace KnuckleBones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        private LabelControl _diceLabel;
        private StackPanelControl _gameOverPanel;
        private LabelControl _winnerLabel;

        public override void Initialize()
        {
            AddControl(0, new LabelControl()
            {
                Text = "KnuckleBones",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                Width = 400,
                Height = 50,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });
            AddControl(0, new LabelControl()
            {
                Text = "Player",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                Width = 90,
                Height = 220,
                X = 310,
                Y = 50,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });
            AddControl(0, new LabelControl()
            {
                Text = "CPU",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                Width = 90,
                Height = 250,
                Y = 580,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            UpdateFirstOpponentBoard();
            UpdateSecondOpponentBoard();
            SetupDice();
            SetupGameOverView();

#if DEBUG
            AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new MainGame(Parent)))
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
            GeneratePointsBox(10, Engine.State.FirstOpponentBoard, 10, 60);
            if (!_controlsLocked && Engine.State.FirstOpponent is PlayerOpponent)
                GenerateSelectionBoxes(10, Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, 60);
            GenerateGrid(10, Engine.State.FirstOpponentBoard, 100, 220, true);
        }

        private void UpdateSecondOpponentBoard()
        {
            ClearLayer(20);
            GeneratePointsBox(20, Engine.State.SecondOpponentBoard, 315, 715);
            if (!_controlsLocked && Engine.State.SecondOpponent is PlayerOpponent)
                GenerateSelectionBoxes(20, Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 100, 100);
            GenerateGrid(20, Engine.State.SecondOpponentBoard, 100, 580);
        }

        private void GeneratePointsBox(int layer, BoardDefinition board, int x, int y)
        {
            AddControl(layer, new LabelControl()
            {
                X = x,
                Y = y,
                Width = 75,
                Height = 75,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                FontColor = Color.Black,
                Text = $"{board.GetValue()}",
                FillColor = BasicTextures.GetBasicRectange(Color.Gold)
            });
        }

        private void GenerateSelectionBoxes(int layer, IOpponent opponent, BoardDefinition board, int x, int y)
        {
            AddControl(layer, new LabelControl()
            {
                X = x + opponent.GetTargetColumn(board) * 75,
                Y = y,
                Width = 50,
                Height = 50 + (board.Columns[0].Cells.Count - 1) * 75 + 20,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Red)
            });
        }

        private void GenerateGrid(int layer, BoardDefinition board, int x, int y, bool flip = false)
        {
            var dir = 1;
            if (flip)
                dir = -1;
            var columnIndex = 0;
            foreach (var column in board.Columns)
            {
                var cellOffset = 0;
                foreach (var cell in column.Cells)
                {
                    var text = "";
                    if (cell != 0)
                        text = $"{cell}";

                    AddControl(layer, new LabelControl()
                    {
                        X = x + columnIndex * 75,
                        Y = y + (cellOffset * 75) * dir,
                        Width = 50,
                        Height = 50,
                        Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                        FontColor = Color.White,
                        Text = text,
                        FillColor = BasicTextures.GetBasicRectange(Color.Gray)
                    });
                    cellOffset++;
                }
                columnIndex++;
            }
        }

        private void SetupDice()
        {
            _diceLabel = new LabelControl()
            {
                X = 159,
                Y = 380,
                Width = 75,
                Height = 75,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
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
                new ButtonControl(Parent, (x) => SwitchView(new MainGame(Parent)))
                {
                    Width = 400,
                    Height = 110,
                    Y = 200,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    Text = "Play Again",
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Black),
                    FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray),
                }
            })
            {
                Y = 270,
                Width = 400,
                Height = 300,
                IsVisible = false
            };
            AddControl(1001, _gameOverPanel);
        }
    }
}