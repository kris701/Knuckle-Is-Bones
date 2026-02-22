using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static MonoGame.OpenGL.Formatter.Controls.StackPanelControl;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
    public partial class SettingsMenu : BaseKnuckleBoneFadeView
    {
        private static List<Point> _resolutionPresets = new List<Point>()
        {
			new Point(2560, 1440),
			new Point(1920, 1080),
            new Point(1600, 900),
            new Point(1440, 810),
            new Point(1280, 720),
            new Point(800, 450),
		};
        private static List<float> _musicPresets = new List<float>()
        {
            1f,
            0.8f,
            0.6f,
            0.4f,
            0.2f,
            0f
        };
        private static List<float> _effectsPresets = new List<float>()
        {
            1f,
            0.8f,
            0.6f,
            0.4f,
            0.2f,
            0f
        };
        private static int _buttonWidth = 250;
		private CanvasPanelControl _resolutionPanel;

		[MemberNotNull(nameof(_resolutionPanel))]
		public override void Initialize()
        {
            AddControl(0, new TileControl()
            {
				Width = IWindow.BaseScreenSize.X,
				Height = IWindow.BaseScreenSize.Y,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            _resolutionPanel = CreateResolutionPanel();
			AddControl(0, new StackPanelControl(new List<IControl>()
			{
				_resolutionPanel,
				CreateOtherCanvas(),
				CreateMusicPanel(),
				CreateEffectsPanel(),
			})
			{
				Width = 1300,
				Height = 800,
				VerticalAlignment = VerticalAlignment.Middle,
				HorizontalAlignment = HorizontalAlignment.Middle
			});

            AddControl(0, new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
            {
                Text = "Cancel",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                Width = 400,
                Height = 100
            });

            AddControl(0, new AnimatedAudioButton(Parent, OnSaveAndApplySettings)
            {
                Text = "Apply",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                Width = 400,
                Height = 100
            });

#if DEBUG
            AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new SettingsMenu(Parent)))
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

        private CanvasPanelControl CreateResolutionPanel()
        {
            var controls = new List<IControl>();
            var selectedTileset = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
            var normalTileset = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
					FontColor = new Color(217, 68, 144),
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

        private CanvasPanelControl CreateOtherCanvas()
        {
            var controls = new List<IControl>();
            var selectedTileset = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
            var normalTileset = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
            controls.Add(new AnimatedAudioButton(Parent, (x) =>
            {
                if (x is AnimatedAudioButton button)
                {
                    _newSettings.IsFullscreen = !_newSettings.IsFullscreen;
                    button.TileSet = _newSettings.IsFullscreen ? selectedTileset : normalTileset;
					_resolutionPanel.IsVisible = !_newSettings.IsFullscreen;
				}
            })
            {
                TileSet = _newSettings.IsFullscreen ? selectedTileset : normalTileset,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
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
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FontColor = Color.White,
                Width = _buttonWidth,
                Text = $"VSync",
            });

            return new CanvasPanelControl(new List<IControl>()
            {
                new LabelControl()
                {
                    Text = "Other",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
					FontColor = new Color(217, 68, 144),
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
            var selectedTileset = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
            var normalTileset = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
					FontColor = new Color(217, 68, 144),
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
            var selectedTileset = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
            var normalTileset = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
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
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
					FontColor = new Color(217, 68, 144),
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
