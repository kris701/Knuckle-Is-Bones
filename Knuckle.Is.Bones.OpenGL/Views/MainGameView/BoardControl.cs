using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Textures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public class BoardControl : CollectionControl
	{
		private readonly BoardDefinition _board;
		private readonly int _rows;
		private readonly int _columns;
		private float _cellWidth;
		private float _cellHeight;
		private float _minSize;
		private readonly float _margin = 5;
		private readonly bool _flip = false;
		private TileControl _columnHighlight;
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
			_flip = flip;
		}

		public override void Initialize()
		{
			_cellWidth = ((Width - _margin) / _columns) - _margin;
			_cellHeight = ((Height - _margin) / _rows) - _margin;
			_minSize = Math.Min(_cellWidth, _cellHeight);
			var font = DeteminteFittingFontSize(_minSize);

			_columnHighlight = new TileControl()
			{
				Height = Height,
				Width = _minSize + _margin * 2,
				X = X,
				Y = Y,
				FillColor = BasicTextures.GetBasicRectange(Color.Red),
				IsVisible = false,
				Alpha = 100
			};
			Children.Add(_columnHighlight);

			var columnIndex = 0;
			foreach (var column in _board.Columns)
			{
				var cellOffset = 0;
				if (_flip)
					cellOffset = _rows - 1;
				foreach (var cell in column.Cells)
				{
					var text = "";
					if (cell != 0)
						text = $"{cell}";

					Children.Add(new AnimatedLabelControl()
					{
						X = X + columnIndex * (_cellWidth + _margin) + _margin + ((_cellWidth - _minSize) / 2),
						Y = Y + (cellOffset * (_cellHeight + _margin) + _margin) + ((_cellHeight - _minSize) / 2),
						Width = _minSize,
						Height = _minSize,
						Font = _parent.Fonts.GetFont(font),
						FontColor = Color.White,
						Text = text,
						TileSet = GetBackgroundForCount(column.Cells, cell)
					});
					if (_flip)
						cellOffset--;
					else
						cellOffset++;
				}
				columnIndex++;
			}
		}

		private Guid DeteminteFittingFontSize(float minSize)
		{
			if (minSize > 75)
				return FontSizes.Ptx24;
			if (minSize > 45)
				return FontSizes.Ptx16;
			return FontSizes.Ptx8;
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
				case >= 4: return _parent.Textures.GetTextureSet(new System.Guid("d02a3300-76fa-4965-9f41-18f4af4832a3"));
			}
			return _parent.Textures.GetTextureSet(new System.Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5"));
		}

		public void HighlightColumn(int columnID)
		{
			_columnHighlight.X = X + (_cellWidth + _margin) * columnID + ((_cellWidth - _minSize) / 2);
			_columnHighlight.IsVisible = true;
		}

		public void HideHighlight()
		{
			_columnHighlight.IsVisible = false;
		}

		public void UpdateBoard()
		{
			var itemIndex = 1;
			foreach (var column in _board.Columns)
			{
				foreach (var cell in column.Cells)
				{
					var child = Children[itemIndex++];
					if (child is AnimatedLabelControl lab)
					{
						var text = "";
						if (cell != 0)
							text = $"{cell}";
						lab.Text = text;
						lab.TileSet = GetBackgroundForCount(column.Cells, cell);
					}
				}
			}
		}
	}
}