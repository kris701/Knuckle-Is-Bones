using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.StartGameView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseView
    {
        public override void Initialize()
        {
            AddControl(0, new ButtonControl(Parent, (x) =>
            {
                SwitchView(new MainGame(Parent, new Core.Models.Saves.GameSaveDefinition("save.json")));
            })
            {
                Text = "Continue",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 100,
                HorizontalAlignment = HorizontalAlignment.Middle,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                IsVisible = File.Exists("save.json"),
                Width = 200,
                Height = 100
            });

            AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new StartGame(Parent)))
            {
                Text = "New Game",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 200,
                HorizontalAlignment = HorizontalAlignment.Middle,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                FillColor = BasicTextures.GetBasicRectange(Color.Gray),
                Width = 200,
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
