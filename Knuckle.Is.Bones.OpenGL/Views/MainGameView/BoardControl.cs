using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Textures;
using Knuckle.Is.Bones.Core.Models.Game;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.MainGameView
{
	public class BoardControl : CollectionControl
	{
		public bool CanSelect { get; set; } = false;

		private readonly BoardDefinition _board;
		private readonly int _rows;
		private readonly int _columns;
		private float _cellWidth;
		private float _cellHeight;
		private float _minSize;
		private readonly float _margin = 5;
		private readonly bool _flip = false;
		private readonly IWindow _parent;

		private readonly List<ButtonControl> _columnHighlights = new List<ButtonControl>();
		private readonly Action<int> _columnSelectAction;
		private readonly List<AnimatedLabelControl> _tiles = new List<AnimatedLabelControl>();
		private AnimatedTileControl _modifyingTile = new AnimatedTileControl();

		public BoardControl(IWindow parent, BoardDefinition board, float x, float y, float width, float height, Action<int> columnSelectAction, bool flip = false)
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
			_columnSelectAction = columnSelectAction;
		}

		public override void Initialize()
		{
			_cellWidth = ((Width - _margin) / _columns) - _margin;
			_cellHeight = ((Height - _margin) / _rows) - _margin;
			_minSize = Math.Min(_cellWidth, _cellHeight);
			var font = DeteminteFittingFontSize(_minSize);

			for (int i = 0; i < _board.Columns.Count; i++)
			{
				var newItem = new ButtonControl(
					_parent,
					(s) =>
					{
						if (s.Tag is int sint)
							_columnSelectAction.Invoke(sint);
					},
					(s) =>
					{
						if (CanSelect)
							s.Alpha = 100;
					},
					(s) =>
					{
						if (CanSelect)
							s.Alpha = 0;
					}
				)
				{
					Height = Height,
					Width = _minSize - _margin * 2,
					X = (_cellWidth + _margin) * i + ((_cellWidth - _minSize) / 2) + _margin,
					FillColor = BasicTextures.GetBasicRectange(Color.Red),
					Alpha = 0,
					IsEnabled = !_board.Columns[i].IsFull(),
					Tag = i
				};
				_columnHighlights.Add(newItem);
				Children.Add(newItem);
			}

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

					var newTile = new AnimatedLabelControl()
					{
						X = columnIndex * (_cellWidth + _margin) + ((_cellWidth - _minSize) / 2),
						Y = (cellOffset * (_cellHeight + _margin)) + ((_cellHeight - _minSize) / 2),
						Width = _minSize,
						Height = _minSize,
						Font = _parent.Fonts.GetFont(font),
						FontColor = Color.White,
						Alpha = 100,
						Text = text,
						TileSet = GetBackgroundForCount(column.Cells, cell)
					};
					Children.Add(newTile);
					_tiles.Add(newTile);
					if (_flip)
						cellOffset--;
					else
						cellOffset++;
				}
				columnIndex++;
			}

			_modifyingTile = new AnimatedTileControl()
			{
				Width = Width,
				Height = Height,
				Alpha = 0,
				TileSet = _parent.Textures.GetTextureSet(TextureHelpers.GameBoardModifying)
			};
			Children.Add(_modifyingTile);

			base.Initialize();
		}

		private Guid DeteminteFittingFontSize(float minSize)
		{
			if (minSize > 75)
				return FontHelpers.Ptx24;
			if (minSize > 45)
				return FontHelpers.Ptx16;
			return FontHelpers.Ptx8;
		}

		private TextureSetDefinition GetBackgroundForCount(List<int> cells, int value)
		{
			if (value == 0)
				return _parent.Textures.GetTextureSet(TextureHelpers.GameSquare);
			switch (cells.Count(x => x == value))
			{
				case 2: return _parent.Textures.GetTextureSet(TextureHelpers.GameCombo2);
				case 3: return _parent.Textures.GetTextureSet(TextureHelpers.GameCombo3);
				case >= 4: return _parent.Textures.GetTextureSet(TextureHelpers.GameCombo4);
			}
			return _parent.Textures.GetTextureSet(TextureHelpers.GameSquare);
		}

		public void HighlightColumn(int columnID)
		{
			foreach (var item in _columnHighlights)
			{
				item.IsEnabled = false;
				item.Alpha = 0;
			}
			_columnHighlights[columnID].IsEnabled = true;
			_columnHighlights[columnID].Alpha = 100;
		}

		public void EnableAll()
		{
			var index = 0;
			foreach (var item in _columnHighlights)
			{
				if (!_board.Columns[index].IsFull())
					item.IsEnabled = true;
				index++;
			}
		}
		public void DisableAll()
		{
			foreach (var item in _columnHighlights)
				item.IsEnabled = false;
		}

		public void HideHighlight()
		{
			foreach (var item in _columnHighlights)
			{
				item.IsEnabled = false;
				item.Alpha = 0;
			}
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var tile in _tiles)
				if (tile.Alpha > 100)
					tile.Alpha -= 2;
			if (_modifyingTile.Alpha > 0)
				_modifyingTile.Alpha -= 6;
			base.Update(gameTime);
		}

		public void UpdateBoard()
		{
			var itemIndex = 0;
			foreach (var column in _board.Columns)
			{
				foreach (var cell in column.Cells)
				{
					var child = _tiles[itemIndex++];
					if (child is AnimatedLabelControl lab)
					{
						var text = "";
						if (cell != 0)
							text = $"{cell}";
						var newTileSet = GetBackgroundForCount(column.Cells, cell);
						if (lab.Text != text || lab.TileSet.ID != newTileSet.ID)
						{
							lab.Text = text;
							lab.TileSet = newTileSet;
							lab.Alpha = 256;
						}
					}
				}
			}
		}

		public void ShowModifying()
		{
			_modifyingTile.Alpha = 150;
		}
	}
}