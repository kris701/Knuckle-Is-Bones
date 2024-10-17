using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
    public class BoardControl : CollectionControl
    {
        private readonly BoardDefinition _board;
        private readonly int _rows;
        private readonly int _columns;
        private readonly float _cellWidth;
        private readonly float _cellHeight;
        private readonly float _margin = 5;
        private readonly TileControl _columnHighlight;

        public BoardControl(IWindow parent, BoardDefinition board, float x, float y, float width, float height, bool flip = false)
        {
            _board = board;
            _columns = board.Columns.Count;
            _rows = board.Columns.Max(x => x.Cells.Count);
            Children = new List<IControl>();
            Width = width;
            Height = height;
            X = x;
            Y = y;

            _cellWidth = ((width - _margin) / _columns) - _margin;
            _cellHeight = ((height - _margin) / _rows) - _margin;

            Children.Add(new TileControl()
            {
                Width = width,
                Height = height,
                X = x,
                Y = y,
                FillColor = BasicTextures.GetBasicRectange(Color.DarkGray)
            });

            _columnHighlight = new TileControl()
            {
                Height = height,
                Width = _cellWidth + _margin * 2,
                X = x,
                Y = y,
                FillColor = BasicTextures.GetBasicRectange(Color.Red),
                IsVisible = false
            };
            Children.Add(_columnHighlight);

            var columnIndex = 0;
            foreach (var column in board.Columns)
            {
                var cellOffset = 0;
                if (flip)
                    cellOffset = _rows - 1;
                foreach (var cell in column.Cells)
                {
                    var text = "";
                    if (cell != 0)
                        text = $"{cell}";

                    Children.Add(new LabelControl()
                    {
                        X = x + columnIndex * (_cellWidth + _margin) + _margin,
                        Y = y + (cellOffset * (_cellHeight + _margin) + _margin),
                        Width = _cellWidth,
                        Height = _cellHeight,
                        Font = parent.Fonts.GetFont(FontSizes.Ptx48),
                        FontColor = Color.White,
                        Text = text,
                        FillColor = GetBackgroundForCount(column.Cells, cell)
                    });
                    if (flip)
                        cellOffset--;
                    else
                        cellOffset++;
                }
                columnIndex++;
            }
        }

        private Texture2D GetBackgroundForCount(List<int> cells, int value)
        {
            if (value == 0)
                return BasicTextures.GetBasicRectange(Color.Gray);
            switch (cells.Count(x => x == value))
            {
                case 0:
                case 1: return BasicTextures.GetBasicRectange(Color.Gray);
                case 2: return BasicTextures.GetBasicRectange(Color.Blue);
                case 3: return BasicTextures.GetBasicRectange(Color.Gold);
            }
            return BasicTextures.GetBasicRectange(Color.Gray);
        }

        public void HighlightColumn(int columnID)
        {
            _columnHighlight.X = X + (_cellWidth + _margin) * columnID;
            _columnHighlight.IsVisible = true;
        }
    }
}