using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
    public partial class StartGame : BaseView
    {
        public static Guid ID = new Guid("9ba30a3d-f77c-4aa4-b390-8c8789dba4c0");

        public StartGame(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }
    }
}
