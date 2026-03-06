using FormMatter.OpenGL.Helpers;
using FormMatter.OpenGL.Input;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SplashScreenView
{
	public partial class SplashScreen : BaseNavigatableView
	{
		private readonly MouseWatcher _mouseClickWatcher;
		private readonly GameTimer _splashTimer;

		public SplashScreen(KnuckleBoneWindow parent) : base(parent, 0)
		{
			parent.Audio.PlaySong(SongHelpers.MainMusic);
			_mouseClickWatcher = new MouseWatcher(SwitchToMainMenu);
			_splashTimer = new GameTimer(TimeSpan.FromSeconds(3), (t) => SwitchView(new MainMenu(parent)));
			BackAction = () => SwitchToMainMenu();
			AcceptAction = () => SwitchToMainMenu();
			ShowControls = false;
			_keyboardNavigator.OnAnyKeyDown = () => { InputType = InputTypes.Keyboard; SwitchToMainMenu(); };
			_gamepadNavigator.OnAnyKeyDown = () => { InputType = InputTypes.Gamepad; SwitchToMainMenu(); };
			Initialize();
		}

		private void SwitchToMainMenu()
		{
			SwitchView(new MainMenu(Parent));
		}

		public override void OnUpdate(GameTime gameTime)
		{
			_mouseClickWatcher.Update();
			_splashTimer.Update(gameTime);
			base.OnUpdate(gameTime);
		}
	}
}
