using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
    public partial class SettingsMenu : BaseFadeView
    {
        public static Guid ID = new Guid("356b5d18-1aaf-4c98-aa73-2b27fe82ed1f");

        public SettingsMenu(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }
    }
}
