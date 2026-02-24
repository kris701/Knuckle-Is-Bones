using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Input;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
	public partial class HowToPlay : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("207df693-09c5-428c-b2ab-513950bf6bd0");
		private readonly KeyWatcher _escapeKeyWatcher;
		public HowToPlay(KnuckleBoneWindow parent) : base(parent, ID)
		{
			_escapeKeyWatcher = new KeyWatcher(Keys.Escape, () => SwitchView(new MainMenu(parent)));
			Initialize();
		}

		public override void OnUpdate(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();
			_escapeKeyWatcher.Update(keyState);
		}
	}
}
