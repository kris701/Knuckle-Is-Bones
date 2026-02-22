using MonoGame.OpenGL.Formatter.Views;
using System;

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
