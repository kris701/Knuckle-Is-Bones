using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using System;

namespace Knuckle.Is.Bones.OpenGL.Controls
{
	public class AnimatedAudioButton : AnimatedButtonControl
	{
		public AnimatedAudioButton(IWindow parent, ButtonControlHandler? clicked = null) : base(parent, clicked)
		{
			ClickSound = SoundEffectHelpers.ClickSound;
		}

		public void DoClickNoSound()
		{
			ClickSound = Guid.Empty;
			DoClick();
			ClickSound = SoundEffectHelpers.ClickSound;
		}
	}
}
