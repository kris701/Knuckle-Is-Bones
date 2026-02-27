using System;

namespace Knuckle.Is.Bones.OpenGL.Helpers
{
	public static class TextureHelpers
	{
		#region Interface

		public static Guid Button => new Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce");
		public static Guid ButtonSmall => new Guid("de7f2a5a-82c7-4700-b2ba-926bceb1689a");
		public static Guid ButtonSmallSelect => new Guid("cfa11efd-0284-4abb-bd12-9df0837081b0");

		#endregion

		#region Transitions

		public static Guid TransitionIn => new Guid("6c17df32-bb0e-47a6-bb90-57c593416782");
		public static Guid TransitionOut => new Guid("fbf34095-55e4-4b14-876e-ce7a2e0756ec");

		#endregion

		#region StartGame

		public static Guid StartGameDescription => new Guid("29744523-5a1b-43cd-abd8-ecb79006d148");
		public static Guid CompletionBronze => new Guid("855231a0-daf4-4a10-b163-8da4fd7ae408");
		public static Guid CompletionSilver => new Guid("2a2e927b-6691-4df8-8de1-f0a819cc37ca");
		public static Guid CompletionGold => new Guid("345434d1-ccfc-458f-90a4-3077094c8b8a");

		#endregion

		#region Shop

		public static Guid ShopItem => new Guid("ac42399b-fecf-4627-96e8-bb188369dc81");
		public static Guid ShopDescription => new Guid("0c464e6e-8fcb-4ba9-838d-0b1e5edfca12");

		#endregion

		#region MainGame

		public static Guid GameBoardModifying => new Guid("6399f667-05f1-4a60-8f2e-dac4b3dbd809");
		public static Guid GameSquare => new Guid("a05f00b0-fcdd-41a8-a350-90bf0956c3b5");
		public static Guid GameCombo2 => new Guid("3365fe57-9c47-44ec-b3d2-457fa0fdf3c4");
		public static Guid GameCombo3 => new Guid("936d00f9-2f70-40f6-9d1b-b13cec0fc54a");
		public static Guid GameCombo4 => new Guid("d02a3300-76fa-4965-9f41-18f4af4832a3");
		public static Guid GamePoints => new Guid("4214d3a5-c6c6-4893-a366-30005537799b");
		public static Guid GameGameOver => new Guid("d7ae88e1-8b8e-4ea9-9c99-e78e2d91943a");

		#endregion

		public static Guid Title => new Guid("af1a8619-0867-44ce-89ab-e2d42912ba44");

	}
}
