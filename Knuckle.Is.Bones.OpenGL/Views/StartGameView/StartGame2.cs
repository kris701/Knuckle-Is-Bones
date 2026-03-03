using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.StartGameView
{
	public partial class StartGame2 : BaseNavigatableView
	{
		private readonly LastGameSetupModel.LastGameSetupType _type;

		public StartGame2(KnuckleBoneWindow parent, LastGameSetupModel.LastGameSetupType type) : base(parent, new Guid("b350e448-f201-46ce-baee-1df03f1dbf5c"), new List<int>() { 0 })
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			AcceptAction = () => Start();
			_type = type;
			Initialize();
		}

		private void Start()
		{
			// ..
		}
	}
}
