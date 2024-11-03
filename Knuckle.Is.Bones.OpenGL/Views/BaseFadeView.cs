using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knuckle.Is.Bones.OpenGL.Views
{
    public abstract class BaseKnuckleBoneFadeView : BaseFadeView
    {
        public new KnuckleBoneWindow Parent { get; set; }
        public BaseKnuckleBoneFadeView(KnuckleBoneWindow parent, Guid id) : base(parent, id)
        {
            Parent = parent;
        }
    }
}
