using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Controls.Elements;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Models;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public class SelectorWheelControl : CollectionControl
	{
		public IDefinition CurrentDefinition { get; private set; }

		private int _wheelSize = 9;
		private readonly List<IDefinition> _definitions;
		private int _index = 0;
		private List<AnimatedLabelControl> _wheelItems = new List<AnimatedLabelControl>();
		private List<AnimatedTileControl> _wheelMedalItems = new List<AnimatedTileControl>();

		private KnuckleBoneWindow _parent;
		private LabelControl _selectedName;
		private TextboxControl _selectedDesc;
		private AnimatedTileControl _selectedIcon;
		private AnimatedTileControl _selectedMedal;

		private ScrollWatcherElement _scrollWatcher;

		public SelectorWheelControl(KnuckleBoneWindow parent, List<IDefinition> definitions, IDefinition? selected)
		{
			_parent = parent;
			_definitions = definitions;
			if (selected != null)
				CurrentDefinition = selected;
			else
				CurrentDefinition = definitions[0];

			_index = definitions.IndexOf(CurrentDefinition);

			Children = new List<IControl>();

			Height = 900;
			Width = 400;
			_selectedName = new LabelControl()
			{
				Font = parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = $"{CurrentDefinition.Name}",
				FontColor = FontHelpers.SecondaryColor,
				Height = 75,
				Y = Height / 2 - 350 / 2,
				Width = Width
			};
			Children.Add(_selectedName);
			_selectedDesc = new TextboxControl()
			{
				Font = parent.Fonts.GetFont(FontHelpers.Ptx8),
				Text = $"{CurrentDefinition.Description}",
				FontColor = FontHelpers.PrimaryColor,
				Height = 125,
				Width = Width,
				Margin = 20,
				Y = Height / 2 - 350 / 2 + 50,
				WordWrap = TextboxControl.WordWrapTypes.Word
			};
			Children.Add(_selectedDesc);
			_selectedMedal = new AnimatedTileControl()
			{
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.CompletionBronze),
				IsVisible = false,
				X = _selectedDesc.X + _selectedDesc.Width,
				Y = _selectedDesc.Y + _selectedDesc.Height / 2 - 50 / 2
			};
			Children.Add(_selectedMedal);
			_selectedIcon = new AnimatedTileControl()
			{
				Y = _selectedDesc.Y + _selectedDesc.Height,
				Height = 150,
				Width = 150,
				X = 125,
				TileSet = _parent.Textures.GetTextureSet(TextureHelpers.Button),
			};
			if (parent.Textures.ContainsTextureSet(CurrentDefinition.ID))
			{
				_selectedIcon.TileSet = parent.Textures.GetTextureSet(CurrentDefinition.ID);
				_selectedIcon.IsVisible = true;
			}
			else
				_selectedIcon.IsVisible = false;
			Children.Add(_selectedIcon);
			UpdateCompletionControl(_selectedMedal, CurrentDefinition.ID);
			Children.Add(new AnimatedTileControl()
			{
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.StartGameDescription),
				Height = 470,
				Y = Height / 2 - 350 / 2 - 60,
				Width = Width
			});

			Children.Add(new AnimatedAudioButton(parent, (s) => MoveDown())
			{
				Height = 50,
				Y = Height / 2 - 350 / 2 - 40,
				Width = Width - 50,
				X = 25,
				FontColor = FontHelpers.SecondaryColor,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.MoveUp),
				Font = parent.Fonts.GetFont(FontHelpers.Ptx16)
			});
			Children.Add(new AnimatedAudioButton(parent, (s) => MoveUp())
			{
				Height = 50,
				Y = Height / 2 + 350 / 2 - 10,
				Width = Width - 50,
				X = 25,
				FontColor = FontHelpers.SecondaryColor,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.MoveDown),
				Font = parent.Fonts.GetFont(FontHelpers.Ptx16)
			});

			var wheelItemHeight = (Height - 350 - 100) / _wheelSize;
			var textureSet = parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			var index = 0;
			var middle = _wheelSize / 2;
			for(int i = 0; i < _wheelSize; i++)
			{
				if (i == middle)
				{
					index++;
					continue;
				}
				var defIndex = GetCircularIndex(i + _index - middle);
				var def = _definitions[defIndex];
				var alpha = index > middle ? (150 / (i - middle)) : (150 / (middle - i));
				var newItem = new AnimatedLabelControl()
				{
					TileSet = textureSet,
					Font = parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = $"{def.Name}",
					FontColor = new Color(alpha, alpha, alpha),
					Tag = def,
					IsVisible = index < _wheelSize,
					Alpha = alpha,
					Y = index * wheelItemHeight + (index >= middle ? 440 : 10),
					Width = Width
				};
				var newMedalItem = new AnimatedTileControl()
				{
					X = newItem.X + newItem.Width,
					Y = newItem.Y,
					TileSet = parent.Textures.GetTextureSet(TextureHelpers.CompletionBronze),
					IsVisible = false
				};
				_wheelItems.Add(newItem);
				_wheelMedalItems.Add(newMedalItem);
				UpdateCompletionControl(newMedalItem, def.ID);
				Children.Add(newMedalItem);
				Children.Add(newItem);
				index++;
			}

			_scrollWatcher = new ScrollWatcherElement(parent);
			_scrollWatcher.ScrollChanged += (o, n) =>
			{
				if (n > o)
					MoveUp();
				else if (n < o)
					MoveDown();
			};
		}

		public override void Initialize()
		{
			base.Initialize();
			_scrollWatcher.X = X;
			_scrollWatcher.Y = Y;
			_scrollWatcher.Width = Width;
			_scrollWatcher.Height = Height;
		}

		public override void Update(GameTime gameTime)
		{
			_scrollWatcher.Update(gameTime);
			base.Update(gameTime);
		}

		private int GetCircularIndex(int value)
		{
			var mod = value % _definitions.Count;
			if (mod == 0)
				return 0;
			if (value < 0)
				return (_definitions.Count) + mod;
			else
				return mod;
		}

		private void MoveUp()
		{
			_index--;
			if (_index < 0)
				_index = _definitions.Count - 1;

			UpdateNames();
		}

		private void MoveDown()
		{
			_index++;
			if (_index >= _definitions.Count)
				_index = 0;

			UpdateNames();
		}

		public void SetByID(Guid id)
		{
			var target = _definitions.FirstOrDefault(x => x.ID == id);
			if (target != null)
			{
				_index = _definitions.IndexOf(target);
				UpdateNames();
			}
		}

		private void UpdateNames()
		{
			var middle = _wheelSize / 2;
			for (int i = 0; i < _wheelSize; i++)
			{
				if (i == middle)
					continue;
				var defIndex = GetCircularIndex(i + _index - middle);
				var def = _definitions[defIndex];
				var wheelIndex = i - (i > middle ? 1 : 0);
				_wheelItems[wheelIndex].Text = def.Name;
				UpdateCompletionControl(_wheelMedalItems[wheelIndex], def.ID);
			}

			CurrentDefinition = _definitions[_index];
			_selectedName.Text = CurrentDefinition.Name;
			_selectedDesc.Text = CurrentDefinition.Description;

			if (_parent.Textures.ContainsTextureSet(CurrentDefinition.ID))
			{
				_selectedIcon.TileSet = _parent.Textures.GetTextureSet(CurrentDefinition.ID);
				_selectedIcon.IsVisible = true;
			}
			else
				_selectedIcon.IsVisible = false;

			UpdateCompletionControl(_selectedMedal, CurrentDefinition.ID);
		}

		private void UpdateCompletionControl(AnimatedTileControl tile, Guid id)
		{
			Guid? tileSet = null;
			if (_parent.User.CompletedItems.ContainsKey(id))
			{
				switch (_parent.User.CompletedItems[id])
				{
					case 1: tileSet = TextureHelpers.CompletionBronze; break;
					case 2: tileSet = TextureHelpers.CompletionSilver; break;
					case >= 3: tileSet = TextureHelpers.CompletionGold; break;
				}
			}
			if (tileSet == null)
				tile.IsVisible = false;
			else
			{
				tile.TileSet = _parent.Textures.GetTextureSet((Guid)tileSet);
				tile.IsVisible = true;
			}
		}
	}
}
