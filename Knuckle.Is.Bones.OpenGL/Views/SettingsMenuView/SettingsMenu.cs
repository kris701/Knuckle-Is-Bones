using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.Screens.SettingsView.AcceptView;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView
{
	public partial class SettingsMenu : BaseNavigatableView
	{
		private readonly SettingsDefinition _newSettings;

		public SettingsMenu(KnuckleBoneWindow parent) : base(parent, new Guid("356b5d18-1aaf-4c98-aa73-2b27fe82ed1f"), new List<int>() { 0 })
		{
			_newSettings = Parent.Settings.Clone();
			BackAction = () => Escape();
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

		private void Escape()
		{
			if (Parent.Settings.Equals(_newSettings))
				SwitchView(new MainMenu(Parent));
			else
				ShowAcceptView();
		}
	}
}
