using FormMatter.OpenGL.Controls;
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
		private int _wheelSize = 9;
		private readonly List<IDefinition> _definitions;
		private IDefinition _currentDefinition;
		private int _index = 0;
		private List<AnimatedLabelControl> _wheelItems = new List<AnimatedLabelControl>();
		private List<AnimatedTileControl> _wheelMedalItems = new List<AnimatedTileControl>();

		private KnuckleBoneWindow _parent;
		private LabelControl _selectedName;
		private TextboxControl _selectedDesc;

		public SelectorWheelControl(KnuckleBoneWindow parent, List<IDefinition> definitions, IDefinition? selected)
		{
			_parent = parent;
			_definitions = definitions;
			if (selected != null)
				_currentDefinition = selected;
			else
				_currentDefinition = definitions[0];

			_index = definitions.IndexOf(_currentDefinition);

			Children = new List<IControl>();

			Height = 900;
			Width = 400;
			_selectedName = new LabelControl()
			{
				Font = parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = $"{_currentDefinition.Name}",
				FontColor = FontHelpers.SecondaryColor,
				Height = 75,
				Y = Height / 2 - 350 / 2,
				Width = Width
			};
			Children.Add(_selectedName);
			_selectedDesc = new TextboxControl()
			{
				Font = parent.Fonts.GetFont(FontHelpers.Ptx8),
				Text = $"{_currentDefinition.Description}",
				FontColor = FontHelpers.PrimaryColor,
				Height = 200,
				Width = Width,
				Y = Height / 2 - 350 / 2 + 75,
				WordWrap = TextboxControl.WordWrapTypes.Word
			};
			Children.Add(_selectedDesc);
			Children.Add(new AnimatedTileControl()
			{
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.StartGameDescription),
				Height = 350,
				Y = Height / 2 - 350 / 2,
				Width = Width
			});

			Children.Add(new AnimatedAudioButton(parent, (s) => MoveUp())
			{
				Height = 50,
				Y = Height / 2 - 350 / 2 - 50,
				Width = Width,
				Text = "^",
				FontColor = FontHelpers.SecondaryColor,
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall),
				Font = parent.Fonts.GetFont(FontHelpers.Ptx16)
			});
			Children.Add(new AnimatedAudioButton(parent, (s) => MoveDown())
			{
				Height = 50,
				Y = Height / 2 + 350 / 2,
				Width = Width,
				Text = "v",
				FontColor = FontHelpers.SecondaryColor,
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall),
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
				var def = _definitions[(i + _index + middle + (_definitions.Count % 2 == 0 ? 0 : 1)) % _definitions.Count];
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
		}

		public void MoveUp()
		{
			_index--;
			if (_index < 0)
				_index = _definitions.Count - 1;

			UpdateNames();
		}

		public void MoveDown()
		{
			_index++;
			if (_index >= _definitions.Count)
				_index = 0;

			UpdateNames();
		}

		private void UpdateNames()
		{
			var middle = _wheelSize / 2;
			for (int i = 0; i < _wheelSize; i++)
			{
				if (i == middle)
					continue;
				var def = _definitions[(i + _index + middle + (_definitions.Count % 2 == 0 ? 0 : 1)) % _definitions.Count];
				var wheelIndex = i - (i > middle ? 1 : 0);
				_wheelItems[wheelIndex].Text = def.Name;
				UpdateCompletionControl(_wheelMedalItems[wheelIndex], def.ID);
			}

			_currentDefinition = _definitions[_index];
			_selectedName.Text = _currentDefinition.Name;
			_selectedDesc.Text = _currentDefinition.Description;
			if (_parent.User.CompletedItems.ContainsKey(_currentDefinition.ID))
				_selectedDesc.Text += $"You have won with this {_parent.User.CompletedItems[_currentDefinition.ID]} times";
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
