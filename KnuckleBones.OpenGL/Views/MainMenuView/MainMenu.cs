using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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