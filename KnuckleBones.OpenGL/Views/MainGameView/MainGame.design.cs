using KnuckleBones.Core.Models.Game;
using KnuckleBones.Core.Models.Game.Opponents;
using KnuckleBones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;

namespace KnuckleBones.OpenGL.Views.MainGameView
{
    public partial class MainGame : BaseView
    {
        public override void Initialize()
        {
            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();

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

        private void UpdateBoard(IOpponent opponent, BoardDefinition board, int offset, bool flip)
        {
            int dir = 1;
            if (flip)
                dir = -1;
            ClearLayer(offset);
            AddControl(offset, new LabelControl()
            {
                X = 25,
                Y = 100 + offset + 75,
                Width = 50,
                Height = 50,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                FontColor = Color.White,
                Text = $"{board.GetValue()}",
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
            if (opponent is PlayerOpponent)
            {
                var yOffset = 0;
                if (flip)
                    yOffset = (board.Columns[0].Cells.Count - 1) * 75;
                AddControl(offset, new LabelControl()
                {
                    X = 100 + opponent.GetTargetColumn(board) * 75,
                    Y = 90 + offset + 75 - yOffset,
                    Width = 50,
                    Height = 50 + (board.Columns[0].Cells.Count - 1) * 75 + 20,
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    FillColor = BasicTextures.GetBasicRectange(Color.Red)
                });
            }
            var columnIndex = 0;
            foreach (var column in board.Columns)
            {
                var cellOffset = 0;
                foreach (var cell in column.Cells)
                {
                    var text = "";
                    if (cell != 0)
                        text = $"{cell}";

                    AddControl(offset, new LabelControl()
                    {
                        X = 100 + columnIndex * 75,
                        Y = 100 + offset + 75 + (cellOffset * 75) * dir,
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

        private void UpdateDice()
        {
            ClearLayer(1);
            AddControl(1, new LabelControl()
            {
                X = 10,
                Y = 100,
                Width = 50,
                Height = 50,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = $"{Engine.State.CurrentDice.Value}",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
        }

        private void ShowGameOverView()
        {
            if (!Engine.GameOver)
                return;

            ClearLayer(2);
            var won = "";
            if (Engine.State.Turn == Engine.State.FirstOpponent.OpponentID)
                won = "First opponent won!";
            else
                won = "Second opponent won!";

            AddControl(2, new LabelControl()
            {
                X = 500,
                Y = 400,
                Width = 500,
                Height = 200,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
                Text = $"Game Over!",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
            AddControl(2, new LabelControl()
            {
                X = 500,
                Y = 600,
                Width = 500,
                Height = 100,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = won,
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
            AddControl(2, new ButtonControl(Parent, (x) => SwitchView(new MainGame(Parent)))
            {
                X = 500,
                Y = 700,
                Width = 500,
                Height = 100,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = "Play Again",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                FillClickedColor = BasicTextures.GetBasicRectange(Color.DarkGray)
            });
        }
    }
}