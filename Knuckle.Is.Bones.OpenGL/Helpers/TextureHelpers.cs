using System;
using System.Collections.Generic;
using System.Text;

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

		public static Guid Title => new Guid("af1a8619-0867-44ce-89ab-e2d42912ba44");

	}
}
