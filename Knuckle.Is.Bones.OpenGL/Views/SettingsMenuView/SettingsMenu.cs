using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.Screens.SettingsView.AcceptView;
using MonoGame.OpenGL.Formatter.Controls;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
	public partial class SettingsMenu : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("356b5d18-1aaf-4c98-aa73-2b27fe82ed1f");

		private readonly SettingsDefinition _newSettings;

		public SettingsMenu(KnuckleBoneWindow parent) : base(parent, ID)
		{
			_newSettings = Parent.User.UIData.Clone();
			Initialize();
		}

		private void OnSaveAndApplySettings(ButtonControl sender)
		{
			var oldSettings = Parent.User.UIData.Clone();
			var newSettings = _newSettings.Clone();
			Parent.User.UIData = newSettings;
			Parent.ApplySettings();
			SwitchView(new AcceptView(Parent, oldSettings, newSettings));
		}
	}
}
