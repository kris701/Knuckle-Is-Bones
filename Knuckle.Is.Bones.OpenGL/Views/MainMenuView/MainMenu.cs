using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
	public partial class MainMenu : BaseNavigatableView
	{
		public MainMenu(KnuckleBoneWindow parent) : base(parent, new Guid("8f655142-92e9-4f6d-ae65-4a26a818c0c0"), new List<int>() { 0 })
		{
			Initialize();
		}
	}
}
