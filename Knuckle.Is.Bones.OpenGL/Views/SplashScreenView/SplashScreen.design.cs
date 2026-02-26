using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using System;

namespace Knuckle.Is.Bones.OpenGL.Views.SplashScreenView
{
	public partial class SplashScreen : BaseKnuckleBoneFadeView
	{
		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			AddControl(0, new AnimatedTileControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				VerticalAlignment = VerticalAlignment.Middle,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Title)
			});
#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new SplashScreen(Parent)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FontColor = Color.Black,
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}
	}
}
