using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.Screens.SettingsView.AcceptView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Input;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
	public partial class SettingsMenu : BaseKnuckleBoneFadeView
	{
		private readonly SettingsDefinition _newSettings;
		private readonly KeyWatcher _escapeKeyWatcher;

		public SettingsMenu(KnuckleBoneWindow parent) : base(parent, new Guid("356b5d18-1aaf-4c98-aa73-2b27fe82ed1f"))
		{
			_newSettings = Parent.Settings.Clone();
			_escapeKeyWatcher = new KeyWatcher(Keys.Escape, Escape);
			Initialize();
		}

		private void OnSaveAndApplySettings(ButtonControl sender) => ShowAcceptView();

		private void ShowAcceptView()
		{
			var oldSettings = Parent.Settings.Clone();
			var newSettings = _newSettings.Clone();
			Parent.Settings = newSettings;
			Parent.ApplySettings();
			SwitchView(new AcceptView(Parent, oldSettings, newSettings));
		}

		public override void OnUpdate(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();
			_escapeKeyWatcher.Update(keyState);
		}

		private void Escape()
		{
			if (Parent.Settings.Equals(_newSettings))
				SwitchView(new MainMenu(Parent));
			else
				ShowAcceptView();
		}
	}
}
