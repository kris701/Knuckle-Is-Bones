using KnuckleBones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Views;

namespace KnuckleBones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseView
    {
        public override void Initialize()
        {
            AddControl(0, new LabelControl()
            {
                HorizontalAlignment = HorizontalAlignment.Middle,
                Y = 1050,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx12),
                Text = "Copyright Kristian Skov Johansen",
                FontColor = Color.White,
            });
            AddControl(0, new LabelControl()
            {
                HorizontalAlignment = HorizontalAlignment.Middle,
                Y = 200,
                Font = Parent.Fonts.GetFont(FontSizes.Ptx72),
                Text = "KnuckleBones",
                FontColor = Color.White,
            });

            base.Initialize();
        }
    }
}