using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseAnimatedView
    {
        public static Guid ID = new Guid("8f655142-92e9-4f6d-ae65-4a26a818c0c0");

        public MainMenu(IWindow parent) : base(
            parent, 
            ID,
            parent.Textures.GetTextureSet(new Guid("aeb7a8dd-d580-4550-af87-1db80e522dfa")),
            parent.Textures.GetTextureSet(new Guid("e650684b-a4cf-4950-990e-60b08aac57c5")))
        {
            Initialize();
        }
    }
}
