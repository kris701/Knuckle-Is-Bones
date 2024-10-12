using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;

namespace KnuckleBones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseView
    {
        public static Guid ID = new Guid("e67f87fd-75a6-4e9f-9e8d-c6dc3317061a");

        public MainMenu(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }
    }
}