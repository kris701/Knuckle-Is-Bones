using Knuckle.Is.Bones.OpenGL.Helpers;
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
			parent.Textures.GetTextureSet(TextureHelpers.TransitionIn),
			parent.Textures.GetTextureSet(TextureHelpers.TransitionOut))
		{
			Parent = parent;
		}
	}
}
