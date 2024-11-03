using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
    public partial class SettingsMenu : BaseKnuckleBoneFadeView
    {
        private static List<Point> _resolutionPresets = new List<Point>()
        {
            new Point(1920, 1080),
            new Point(1068, 600)
        };

        public override void Initialize()
        {
            AddControl(0, new TileControl()
            {
                Width = 1920,
                Height = 1080,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            AddControl(0, new StackPanelControl(new List<IControl>()
            {
                CreateResolutionPanel(900)
            })
            {
                Width = 900,
                Height = 800,
                VerticalAlignment = VerticalAlignment.Middle,
                HorizontalAlignment = HorizontalAlignment.Middle
            });

            AddControl(0, new AnimatedButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
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

            AddControl(0, new AnimatedButtonControl(Parent, OnSaveAndApplySettings)
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

        private CanvasPanelControl CreateResolutionPanel(int width)
        {
            var controls = new List<IControl>();
            controls.Add(new LabelControl()
            {
                Text = "Resolution",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                FontColor = Color.White,
                Height = 100,
            });
            var selectedTileset = Parent.Textures.GetTextureSet(new System.Guid("cfa11efd-0284-4abb-bd12-9df0837081b0"));
            var normalTileset = Parent.Textures.GetTextureSet(new System.Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a"));
            int index = 0;
            foreach (var opt in _resolutionPresets)
            {
                controls.Add(new AnimatedButtonControl(Parent, (x) =>
                {
                    foreach (var control in controls)
                        if (control is AnimatedButtonControl other)
                            other.TileSet = normalTileset;
                    if (x is AnimatedButtonControl button)
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
                    Y = 100,
                    X = index * 240,
                    Width = 220,
                    Height = 50,
                    Text = $"{opt.X}x{opt.Y}",
                    Tag = opt
                });
                index++;
            }

            return new CanvasPanelControl(controls) 
            {
                Height = 150,
            };
        }

        private bool IsResolutionSelected(Point point) => _newSettings.ResolutionX == point.X && _newSettings.ResolutionY == point.Y;
    }
}
