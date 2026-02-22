using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Textures;
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
		private readonly IWindow _parent;

		public BoardControl(IWindow parent, BoardDefinition board, float x, float y, float width, float height, bool flip = false)
		{
			_parent = parent;
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

					Children.Add(new AnimatedLabelControl()
					{
						X = x + columnIndex * (_cellWidth + _margin) + _margin,
						Y = y + (cellOffset * (_cellHeight + _margin) + _margin),
						Width = _cellWidth,
						Height = _cellHeight,
						Font = parent.Fonts.GetFont(FontSizes.Ptx24),
						FontColor = Color.White,
						Text = text,
						TileSet = GetBackgroundForCount(column.Cells, cell)
					});
					if (flip)
						cellOffset--;
					else
						cellOffset++;
				}
				columnIndex++;
			}
		}

		private TextureSetDefinition GetBackgroundForCount(List<int> cells, int value)
		{
			if (value == 0)
				return _parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"));
			switch (cells.Count(x => x == value))
			{
				case 0:
				case 1: return _parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"));
				case 2: return _parent.Textures.GetTextureSet(new System.Guid("936d00f9-2f70-40f6-9d1b-b13cec0fc54a"));
				case 3: return _parent.Textures.GetTextureSet(new System.Guid("d02a3300-76fa-4965-9f41-18f4af4832a3"));
				case 4: return _parent.Textures.GetTextureSet(new System.Guid("d02a3300-76fa-4965-9f41-18f4af4832a3"));
			}
			return _parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"));
		}

		public void HighlightColumn(int columnID)
		{
			_columnHighlight.X = X + (_cellWidth + _margin) * columnID;
			_columnHighlight.IsVisible = true;
		}
	}
}