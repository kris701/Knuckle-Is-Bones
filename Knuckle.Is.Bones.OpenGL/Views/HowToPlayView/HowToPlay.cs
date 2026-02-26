using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
	public partial class HowToPlay : BaseNavigatableView
	{
		public HowToPlay(KnuckleBoneWindow parent) : base(parent, new Guid("207df693-09c5-428c-b2ab-513950bf6bd0"), new List<int>() { 0 })
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			ShowGeneralControls = false;
			Initialize();
		}
	}
}
