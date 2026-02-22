using System;

namespace Knuckle.Is.Bones.OpenGL.Views.HowToPlayView
{
	public partial class HowToPlay : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("207df693-09c5-428c-b2ab-513950bf6bd0");
		public HowToPlay(KnuckleBoneWindow parent) : base(parent, ID)
		{
			Initialize();
		}
	}
}
