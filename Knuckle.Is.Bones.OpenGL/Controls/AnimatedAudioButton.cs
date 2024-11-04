using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Controls
{
    public class AnimatedAudioButton : AnimatedButtonControl
    {
        public AnimatedAudioButton(IWindow parent, ClickedHandler? clicked = null) : base(parent, clicked)
        {
            ClickSound = new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028");
        }
    }
}
