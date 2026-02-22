using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace Knuckle.Is.Bones.OpenGL.Screens.SettingsView.AcceptView
{
	public partial class AcceptView : BaseKnuckleBoneFadeView
	{
		private LabelControl _timeLeftLabel;

		[MemberNotNull(nameof(_timeLeftLabel))]
		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = IWindow.BaseScreenSize.X,
				Height = IWindow.BaseScreenSize.Y,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			AddControl(0, new LabelControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 100,
				Height = 75,
				Width = 800,
				Text = "Accept Changes?",
				FontColor = new Color(217, 68, 144),
				Font = Parent.Fonts.GetFont(FontSizes.Ptx48)
			});
			AddControl(0, new LabelControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 175,
				Height = 35,
				Width = 700,
				Text = $"Settings will reset in 10 seconds otherwise.",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				FontColor = Color.White
			});
			_timeLeftLabel = new LabelControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 500,
				Text = "10 seconds left",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx48),
				FontColor = Color.Red
			};
			AddControl(0, _timeLeftLabel);


			AddControl(0, new AnimatedAudioButton(Parent, (e) => Cancel())
			{
				Text = "Cancel",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Left,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
				Width = 400,
				Height = 100
			});

			AddControl(0, new AnimatedAudioButton(Parent, (e) => Accept())
			{
				Text = "Accept",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Right,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
				Width = 400,
				Height = 100
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new AcceptView(Parent, _oldSettings, _newSettings)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}
	}
}
