using MonoGame.OpenGL.Formatter.Views;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views
{
	public abstract class BaseKnuckleBoneFadeView : BaseAnimatedView
	{
		public new KnuckleBoneWindow Parent { get; set; }
		public BaseKnuckleBoneFadeView(KnuckleBoneWindow parent, Guid id) : base(
			parent,
			id,
			parent.Textures.GetTextureSet(new Guid("6c17df32-bb0e-47a6-bb90-57c593416782")),
			parent.Textures.GetTextureSet(new Guid("fbf34095-55e4-4b14-876e-ce7a2e0756ec")))
		{
			Parent = parent;
		}
	}
}
