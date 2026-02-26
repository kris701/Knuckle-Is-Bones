using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Models;
using Knuckle.Is.Bones.OpenGL.Views;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FormMatter.OpenGL.Input;
using System;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Screens.SettingsView.AcceptView
{
	public partial class AcceptView : BaseNavigatableView
	{
		private readonly SettingsDefinition _newSettings;
		private readonly SettingsDefinition _oldSettings;
		private TimeSpan _waitFor = TimeSpan.FromSeconds(10);
		private int _secsLeft = 10;
		public AcceptView(KnuckleBoneWindow parent, SettingsDefinition oldSettings, SettingsDefinition newSettings) : base(parent, new Guid("937b4268-87fc-4f72-a180-e53ebd47a18d"), new List<int>() { 0 })
		{
			_oldSettings = oldSettings;
			_newSettings = newSettings;
			BackAction = () => Cancel();
			AcceptAction = () => Accept();
			ShowGeneralControls = false;
			Initialize();
		}

		public override void OnUpdate(GameTime gameTime)
		{
			_waitFor -= gameTime.ElapsedGameTime;
			if (_waitFor.Seconds != _secsLeft)
			{
				_secsLeft = _waitFor.Seconds;
				_timeLeftLabel.Text = $"{_secsLeft} seconds left";
			}
			if (_waitFor <= TimeSpan.Zero)
				Cancel();
			base.OnUpdate(gameTime);
		}

		private void Accept()
		{
			Parent.Settings = _newSettings;
			Parent.ApplySettings();
			SettingsSaveHelpers.Save(Parent.Settings);
			SwitchView(new MainMenu(Parent));
		}

		private void Cancel()
		{
			Parent.Settings = _oldSettings;
			Parent.ApplySettings();
			SettingsSaveHelpers.Save(Parent.Settings);
			SwitchView(new MainMenu(Parent));
		}
	}
}
