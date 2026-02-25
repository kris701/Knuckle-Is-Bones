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
							s.IsVisible = true;
					},
					(s) =>
					{
						if (CanSelect)
							s.IsVisible = false;
					}
				)
				{
					Height = Height,
					Width = _minSize - _margin * 2,
					X = (_cellWidth + _margin) * i + ((_cellWidth - _minSize) / 2) + _margin,
					FillColor = BasicTextures.GetBasicRectange(Color.Red),
					IsVisible = false,
					Alpha = 100,
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
				TileSet = _parent.Textures.GetTextureSet(new Guid("6399f667-05f1-4a60-8f2e-dac4b3dbd809"))
			};
			Children.Add(_modifyingTile);

			base.Initialize();
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
			foreach (var item in _columnHighlights)
				item.IsVisible = false;
			_columnHighlights[columnID].IsVisible = true;
		}

		public void HideHighlight()
		{
			foreach (var item in _columnHighlights)
				item.IsVisible = false;
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
						if ((lab.Text != text || lab.TileSet.ID != newTileSet.ID) && lab.Alpha == 100)
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