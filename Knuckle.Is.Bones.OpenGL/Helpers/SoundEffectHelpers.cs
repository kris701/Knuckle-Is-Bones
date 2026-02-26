using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class SoundEffectHelpers
	{
		public static Guid ShopBuySound => new Guid("83fe89c5-745e-4f76-aa87-02ed76b37b1b");
		public static Guid GameOnDiceRemove => new Guid("4e53cd32-7af6-47a1-a331-ec2096505c78");
		public static Guid GameOnCombo => new Guid("74ea48c8-cb6f-4a22-8226-e5d6142b1f76");
		public static Guid GameOnTurn => new Guid("23ac297f-3e68-461f-a869-a304e89e18c6");
		public static Guid GameOnBoardModified => new Guid("97b1fabe-d7c8-44fc-86bf-94592a91edf8");
		public static Guid GameOnDiceRoll => new Guid("adb4826c-ae62-4785-b0f3-81dd4d692920");
		public static Guid GameOnMove => new Guid("19268829-42c3-411d-8357-91d55de0cef6");
	}
}
