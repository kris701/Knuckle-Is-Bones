using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
	public partial class HowToPlay : BaseNavigatableView
	{
		public HowToPlay(KnuckleBoneWindow parent) : base(parent, 0)
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			ShowGeneralControls = false;
			Initialize();
		}
	}
}
