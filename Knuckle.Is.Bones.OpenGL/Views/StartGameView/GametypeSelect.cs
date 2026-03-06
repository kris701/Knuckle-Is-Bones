using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class GametypeSelect : BaseNavigatableView
	{
		public GametypeSelect(KnuckleBoneWindow parent) : base(parent, 0)
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			Initialize();
		}
	}
}
