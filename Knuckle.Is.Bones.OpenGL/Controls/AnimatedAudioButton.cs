using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using System;

namespace Knuckle.Is.Bones.OpenGL.Controls
{
	public class AnimatedAudioButton : AnimatedButtonControl
	{
		public AnimatedAudioButton(IWindow parent, ButtonControlHandler? clicked = null) : base(parent, clicked)
		{
			ClickSound = new Guid("19f2fb41-6cd2-4c59-ad74-6a15773f4028");
		}
	}
}
