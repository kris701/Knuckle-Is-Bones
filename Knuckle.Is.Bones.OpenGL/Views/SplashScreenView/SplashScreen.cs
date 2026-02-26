using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using FormMatter.OpenGL.Input;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SplashScreenView
{
	public partial class SplashScreen : BaseKnuckleBoneFadeView
	{
		private readonly MouseWatcher _mouseWatcher;
		private readonly GameTimer _splashTimer;

		public SplashScreen(KnuckleBoneWindow parent) : base(parent, new Guid("20aed282-f7e1-4595-9f53-85d95c0efe6f"))
		{
			parent.Audio.PlaySong(SongHelpers.MainMusic);
			_mouseWatcher = new MouseWatcher(SwitchToMainMenu);
			_splashTimer = new GameTimer(TimeSpan.FromSeconds(3), (t) => SwitchView(new MainMenu(parent)));
			Initialize();
		}

		private void SwitchToMainMenu()
		{
			SwitchView(new MainMenu(Parent));
		}

		public override void OnUpdate(GameTime gameTime)
		{
			_mouseWatcher.Update();
			_splashTimer.Update(gameTime.ElapsedGameTime);
		}
	}
}
