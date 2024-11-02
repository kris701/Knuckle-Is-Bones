using Knuckle.Is.Bones.Core.Engines;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Saves;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Input;
using MonoGame.OpenGL.Formatter;
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
        public static Guid ID = new Guid("207df693-09c5-428c-b2ab-513950bf6bd0");
        public HowToPlay(IWindow parent) : base(parent, ID)
        {
            Initialize();
        }
    }
}
