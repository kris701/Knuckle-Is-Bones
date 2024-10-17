using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
    public partial class MainMenu : BaseView
    {
        public static Guid ID = new Guid("8f655142-92e9-4f6d-ae65-4a26a818c0c0");

        public MainMenu(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }
    }
}
