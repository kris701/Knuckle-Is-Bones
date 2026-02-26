using FormMatter.OpenGL.Views;
using Knuckle.Is.Bones.OpenGL.Helpers;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views
{
	public abstract class BaseTransitionView : BaseAnimatedView
	{
		public new KnuckleBoneWindow Parent { get; set; }
		public BaseTransitionView(KnuckleBoneWindow parent, Guid id) : base(
			parent,
			id,
			parent.Textures.GetTextureSet(TextureHelpers.TransitionIn),
			parent.Textures.GetTextureSet(TextureHelpers.TransitionOut))
		{
			Parent = parent;
		}
	}
}
