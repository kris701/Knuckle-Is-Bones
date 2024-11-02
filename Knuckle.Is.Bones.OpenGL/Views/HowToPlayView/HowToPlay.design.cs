using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
    public partial class HowToPlay : BaseFadeView
    {
        public override void Initialize()
        {
            AddControl(0, new TileControl()
            {
                Width = 1920,
                Height = 1080,
                FillColor = BasicTextures.GetBasicRectange(Color.Black)
            });
            // https://cult-of-the-lamb.fandom.com/wiki/Knucklebones
            AddControl(0, new StackPanelControl(new List<IControl>()
            {
                new TextboxControl()
                {
                    Text = $"* The players take turns. On a player's turn, they roll a single 6-sided die, and must place it in a column on their board. A filled column does not accept any more dice.",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    Height = 100,
                },
                new TextboxControl()
                {
                    Text = $"* Each player has a score, which is the sum of all the dice values on their board.",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    Height = 100,
                },
                new TextboxControl()
                {
                    Text = $"* If a player places multiple dice of the same value in the same column, the score awarded for each of those dice is multiplied by the number of dice of the same value in that column. e.g. if a column contains 4-1-4, then the score for that column is 4x2 + 1x1 + 4x2 = 17.",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    Height = 150,
                },
                new TextboxControl()
                {
                    Text = $"* When a player places a die, all dice of the same value in the corresponding column of the opponent's board gets destroyed. Players can use this mechanic to destroy their opponent's high-scoring combos.",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    Height = 150,
                },
                new TextboxControl()
                {
                    Text = $"* The game ends when either player completely fills up their 3x3 board. The player with the higher score wins.",
                    Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
                    FontColor = Color.White,
                    Height = 100,
                }
            })
            {
                Width = 1000,
                Height = 700,
                HorizontalAlignment = HorizontalAlignment.Middle,
                VerticalAlignment = VerticalAlignment.Middle,
            });
            AddControl(0, new AnimatedButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
            {
                Text = "Back",
                Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
                Y = 980,
                HorizontalAlignment = HorizontalAlignment.Left,
                FillClickedColor = BasicTextures.GetClickedTexture(),
                TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
                Width = 400,
                Height = 100
            });

#if DEBUG
            AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new HowToPlay(Parent)))
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
