using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class GametypeSelect : BaseNavigatableView
	{
		public GametypeSelect(KnuckleBoneWindow parent) : base(parent, new Guid("7aeab111-d1d4-4ba1-b35a-92d0e1757bf1"), new List<int>() { 0 })
		{
			BackAction = () => SwitchView(new MainMenu(Parent));

			Initialize();
		}
	}
}
