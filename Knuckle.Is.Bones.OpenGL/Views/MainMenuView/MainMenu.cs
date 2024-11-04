using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseKnuckleBoneFadeView
    {
        public static Guid ID = new Guid("8f655142-92e9-4f6d-ae65-4a26a818c0c0");

        public MainMenu(KnuckleBoneWindow parent) : base(parent, ID)
        {
            parent.Audio.PlaySong(new Guid("9b223250-88f1-4c3e-9d80-0c4087ab9369"));
            Initialize();
        }
    }
}
