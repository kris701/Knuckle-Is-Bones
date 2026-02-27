using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static FormMatter.OpenGL.Controls.StackPanelControl;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
	public partial class SettingsMenu : BaseNavigatableView
	{
		private static readonly List<Point> _resolutionPresets = new List<Point>()
		{
			new Point(2560, 1440),
			new Point(1920, 1080),
			new Point(1600, 900),
			new Point(1440, 810),
			new Point(1280, 720),
			new Point(800, 450),
		};
		private static readonly List<float> _musicPresets = new List<float>()
		{
			1f,
			0.8f,
			0.6f,
			0.4f,
			0.2f,
			0f
		};
		private static readonly List<float> _effectsPresets = new List<float>()
		{
			1f,
			0.8f,
			0.6f,
			0.4f,
			0.2f,
			0f
		};
		private static readonly List<int> _gamepadIndexes = new List<int>()
		{
			0,
			1,
			2,
			3,
			4,
			5
		};
		private static readonly int _buttonWidth = 250;
		private StackPanelControl _settingsPanel;
		private CanvasPanelControl _resolutionPanel;

		[MemberNotNull(nameof(_resolutionPanel), nameof(_settingsPanel))]
		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = IWindow.BaseScreenSize.X,
				Height = IWindow.BaseScreenSize.Y,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			_resolutionPanel = CreateResolutionPanel();
			CreateMainSettingsPanel();
#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new SettingsMenu(Parent)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FontColor = Color.Black,
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}

		[MemberNotNull(nameof(_settingsPanel))]
		private void CreateMainSettingsPanel()
		{
			_settingsPanel = new StackPanelControl(new List<IControl>()
			{
				CreateGeneralPanel(),
				_resolutionPanel,
				CreateMusicPanel(),
				CreateEffectsPanel(),
				CreateGameSpeedPanel(),
			})
			{
				Width = 1300,
				Height = 800,
				VerticalAlignment = VerticalAlignment.Middle,
				HorizontalAlignment = HorizontalAlignment.Middle
			};
			AddControl(0, _settingsPanel);
		}

		private CanvasPanelControl CreateResolutionPanel()
		{
			var controls = new List<IControl>();
			var selectedTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
			var normalTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			foreach (var opt in _resolutionPresets)
			{
				controls.Add(new AnimatedAudioButton(Parent, (x) =>
				{
					foreach (var control in controls)
						if (control is AnimatedAudioButton other)
							other.TileSet = normalTileset;
					if (x is AnimatedAudioButton button)
					{
						button.TileSet = selectedTileset;
						if (x.Tag is Point res)
						{
							_newSettings.ResolutionX = res.X;
							_newSettings.ResolutionY = res.Y;
						}
					}
				})
				{
					TileSet = IsResolutionSelected(opt) ? selectedTileset : normalTileset,
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FontColor = Color.White,
					Width = _buttonWidth,
					Text = $"{opt.X}x{opt.Y}",
					Tag = opt
				});
			}

			return new CanvasPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Resolution",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					FontColor = FontHelpers.SecondaryColor,
					Height = 100,
				},
				new StackPanelControl(controls)
				{
					Height = 50,
					Y = 100,
					Gap = 10,
					Orientation = Orientations.Horizontal,
				}
			})
			{
				Height = 150,
				IsVisible = !_newSettings.IsFullscreen
			};
		}

		private bool IsResolutionSelected(Point point) => _newSettings.ResolutionX == point.X && _newSettings.ResolutionY == point.Y;

		private CanvasPanelControl CreateGeneralPanel()
		{
			var controls = new List<IControl>();
			var selectedTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
			var normalTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			controls.Add(new AnimatedAudioButton(Parent, (x) =>
			{
				if (x is AnimatedAudioButton button)
				{
					_newSettings.IsFullscreen = !_newSettings.IsFullscreen;
					button.TileSet = _newSettings.IsFullscreen ? selectedTileset : normalTileset;
					_resolutionPanel.IsVisible = !_newSettings.IsFullscreen;
					_settingsPanel.Initialize();
				}
			})
			{
				TileSet = _newSettings.IsFullscreen ? selectedTileset : normalTileset,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				FontColor = Color.White,
				Width = _buttonWidth,
				Text = $"Fullscreen",
			});
			controls.Add(new AnimatedAudioButton(Parent, (x) =>
			{
				if (x is AnimatedAudioButton button)
				{
					_newSettings.IsVsync = !_newSettings.IsVsync;
					button.TileSet = _newSettings.IsVsync ? selectedTileset : normalTileset;
				}
			})
			{
				TileSet = _newSettings.IsVsync ? selectedTileset : normalTileset,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				FontColor = Color.White,
				Width = _buttonWidth,
				Text = $"VSync",
			});

			return new CanvasPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "General",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					FontColor = FontHelpers.SecondaryColor,
					Height = 100,
				},
				new StackPanelControl(controls)
				{
					Height = 50,
					Y = 100,
					Gap = 10,
					Orientation = Orientations.Horizontal,
				}
			})
			{
				Height = 150,
			};
		}

		private CanvasPanelControl CreateMusicPanel()
		{
			var controls = new List<IControl>();
			var selectedTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
			var normalTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			foreach (var opt in _musicPresets)
			{
				controls.Add(new AnimatedAudioButton(Parent, (x) =>
				{
					foreach (var control in controls)
						if (control is AnimatedAudioButton other)
							other.TileSet = normalTileset;
					if (x is AnimatedAudioButton button)
					{
						button.TileSet = selectedTileset;
						if (x.Tag is float vol)
							_newSettings.MusicVolume = vol;
					}
				})
				{
					TileSet = _newSettings.MusicVolume == opt ? selectedTileset : normalTileset,
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FontColor = Color.White,
					Width = _buttonWidth,
					Text = $"{Math.Round(opt * 100, 0)}%",
					Tag = opt
				});
			}

			return new CanvasPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Music",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					FontColor = FontHelpers.SecondaryColor,
					Height = 100,
				},
				new StackPanelControl(controls)
				{
					Height = 50,
					Y = 100,
					Gap = 10,
					Orientation = Orientations.Horizontal,
				}
			})
			{
				Height = 150,
			};
		}

		private CanvasPanelControl CreateEffectsPanel()
		{
			var controls = new List<IControl>();
			var selectedTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
			var normalTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			foreach (var opt in _effectsPresets)
			{
				controls.Add(new AnimatedAudioButton(Parent, (x) =>
				{
					foreach (var control in controls)
						if (control is AnimatedAudioButton other)
							other.TileSet = normalTileset;
					if (x is AnimatedAudioButton button)
					{
						button.TileSet = selectedTileset;
						if (x.Tag is float vol)
							_newSettings.EffectsVolume = vol;
					}
				})
				{
					TileSet = _newSettings.EffectsVolume == opt ? selectedTileset : normalTileset,
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FontColor = Color.White,
					Width = _buttonWidth,
					Text = $"{Math.Round(opt * 100, 0)}%",
					Tag = opt
				});
			}

			return new CanvasPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Sound Effects",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					FontColor = FontHelpers.SecondaryColor,
					Height = 100,
				},
				new StackPanelControl(controls)
				{
					Height = 50,
					Y = 100,
					Gap = 10,
					Orientation = Orientations.Horizontal,
				}
			})
			{
				Height = 150,
			};
		}

		private CanvasPanelControl CreateGameSpeedPanel()
		{
			var controls = new List<IControl>();
			var selectedTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmallSelect);
			var normalTileset = Parent.Textures.GetTextureSet(TextureHelpers.ButtonSmall);
			foreach (var opt in Enum.GetValues<GameSpeedTypes>())
			{
				controls.Add(new AnimatedAudioButton(Parent, (x) =>
				{
					foreach (var control in controls)
						if (control is AnimatedAudioButton other)
							other.TileSet = normalTileset;
					if (x is AnimatedAudioButton button)
					{
						button.TileSet = selectedTileset;
						if (x.Tag is GameSpeedTypes speed)
							_newSettings.GameSpeed = speed;
					}
				})
				{
					TileSet = _newSettings.GameSpeed == opt ? selectedTileset : normalTileset,
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FontColor = Color.White,
					Width = _buttonWidth,
					Text = $"{Enum.GetName(opt)}",
					Tag = opt
				});
			}

			return new CanvasPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Game Speed",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					FontColor = FontHelpers.SecondaryColor,
					Height = 100,
				},
				new StackPanelControl(controls)
				{
					Height = 50,
					Y = 100,
					Gap = 10,
					Orientation = Orientations.Horizontal,
				}
			})
			{
				Height = 150,
			};
		}
	}
}
