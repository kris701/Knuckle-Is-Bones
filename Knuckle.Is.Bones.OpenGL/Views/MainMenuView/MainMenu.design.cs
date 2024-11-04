using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.HowToPlayView;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView;
using Knuckle.Is.Bones.OpenGL.Views.StartGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.IO;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseKnuckleBoneFadeView
    {
        public override void Initialize()
        {
            AddControl(0, new TileControl()
            {
                Width = 1920,
                Height = 1080,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });

            AddControl(0, new AnimatedTileControl()
            {
                Y = 25,
                HorizontalAlignment = HorizontalAlignment.Middle,
                Width = 800,
                Height = 264,
                TileSet = Parent.Textures.GetTextureSet(new Guid("af1a8619-0867-44ce-89ab-e2d42912ba44"))
            });

            AddControl(0, new StackPanelControl(new System.Collections.Generic.List<IControl>()
            {
                new AnimatedAudioButton(Parent, (x) =>
                {
                    SwitchView(new MainGame(Parent, new Core.Models.Saves.GameSaveDefinition("save.json")));
                })
                {
                    Text = "Continue",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    IsVisible = File.Exists("save.json"),
                    Height = 100
                },
                new AnimatedAudioButton(Parent, (x) => SwitchView(new StartGame(Parent)))
                {
                    Text = "New Game",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    Height = 100
                },
                new AnimatedAudioButton(Parent, (x) => SwitchView(new HowToPlay(Parent)))
                {
                    Text = "How to Play",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    Height = 100
                },
                new AnimatedAudioButton(Parent, (x) => SwitchView(new SettingsMenu(Parent)))
                {
                    Text = "Settings",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    Height = 100
                },
                new AnimatedAudioButton(Parent, (x) => (Parent as KnuckleBoneWindow).Exit())
                {
                    Text = "Exit",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                    HorizontalAlignment = HorizontalAlignment.Middle,
                    FillClickedColor = BasicTextures.GetClickedTexture(),
                    TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                    Height = 100
                }
            })
            {
                Y = 300,
                HorizontalAlignment = HorizontalAlignment.Middle,
                Width = 400,
                Height = 500
            });

            AddControl(0, new LabelControl()
            {
                Text = $"Points: {(Parent as KnuckleBoneWindow).User.AllTimeScore}",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 500,
                Height = 100
            });

            AddControl(0, new LabelControl()
            {
                Text = "Made by Kristian Skov",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 500,
                Height = 100
            });

#if DEBUG
            AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new MainMenu(Parent)))
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
    }
}
