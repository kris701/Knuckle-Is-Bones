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
            AddControl(0, new ButtonControl(Parent, (x) => TakeTurn())
            {
                X = 350,
                Y = 100,
                Width = 200,
                Height = 50,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = "Take Turn",
                FontColor = Color.White,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });

            UpdateBoard(Engine.State.FirstOpponent, Engine.State.FirstOpponentBoard, 100, true);
            UpdateBoard(Engine.State.SecondOpponent, Engine.State.SecondOpponentBoard, 300, false);
            UpdateDice();

            base.Initialize();
        }

        public void UpdateBoard(IOpponent opponent, BoardDefinition board, int offset, bool flip)
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
            if (opponent is PlayerOpponent player)
            {
                var columnIndex = 0;
                foreach (var column in board.Columns)
                {
                    var cellOffset = 0;
                    foreach (var cell in column.Cells)
                    {
                        AddControl(offset, new ButtonControl(Parent, (x) => player.SetTargetColumn((int)x.Tag))
                        {
                            X = 100 + columnIndex * 75,
                            Y = 100 + offset + 75 + (cellOffset * 75) * dir,
                            Width = 50,
                            Height = 50,
                            Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                            FontColor = Color.White,
                            Text = $"{cell}",
                            FillClickedColor = BasicTextures.GetClickedTexture(),
                            FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                            Tag = columnIndex
                        });
                        cellOffset++;
                    }
                    columnIndex++;
                }
            }
            else
            {
                var columnIndex = 0;
                foreach (var column in board.Columns)
                {
                    var cellOffset = 0;
                    foreach (var cell in column.Cells)
                    {
                        AddControl(offset, new LabelControl()
                        {
                            X = 100 + columnIndex * 75,
                            Y = 100 + offset + 75 + (cellOffset * 75) * dir,
                            Width = 50,
                            Height = 50,
                            Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                            FontColor = Color.White,
                            Text = $"{cell}",
                            FillColor = BasicTextures.GetBasicRectange(Color.Gray)
                        });
                        cellOffset++;
                    }
                    columnIndex++;
                }
            }
        }

        public void UpdateDice()
        {
            ClearLayer(1);
            AddControl(1, new LabelControl()
            {
                X = 10,
                Y = 10,
                Width = 50,
                Height = 50,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Text = $"{Engine.State.CurrentDice.Value}",
                FontColor = Color.White,
                FillColor = BasicTextures.GetBasicRectange(Color.Gray)
            });
        }
    }
}