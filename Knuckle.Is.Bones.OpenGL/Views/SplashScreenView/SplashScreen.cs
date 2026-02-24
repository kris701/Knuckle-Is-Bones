using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Input;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SplashScreenView
{
	public partial class SplashScreen : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("20aed282-f7e1-4595-9f53-85d95c0efe6f");

		private readonly KeyWatcher _enterKeyWatcher;
		private readonly GameTimer _splashTimer;

		public SplashScreen(KnuckleBoneWindow parent) : base(parent, ID)
		{
			parent.Audio.PlaySong(new Guid("9b223250-88f1-4c3e-9d80-0c4087ab9369"));
			_enterKeyWatcher = new KeyWatcher(Keys.Enter, () => SwitchView(new MainMenu(parent)));
			_splashTimer = new GameTimer(TimeSpan.FromSeconds(3), (t) => SwitchView(new MainMenu(parent)));
			Initialize();
		}

		public override void OnUpdate(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();
			_enterKeyWatcher.Update(keyState);
			_splashTimer.Update(gameTime.ElapsedGameTime);
		}
	}
}
