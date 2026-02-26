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
				FontColor = FontHelpers.SecondaryColor,
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx48)
			});
			AddControl(0, new LabelControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 175,
				Height = 35,
				Width = 700,
				Text = $"Settings will reset in 10 seconds otherwise.",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				FontColor = FontHelpers.PrimaryColor
			});
			_timeLeftLabel = new LabelControl()
			{
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 500,
				Text = "10 seconds left",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx48),
				FontColor = FontHelpers.ErrorColor
			};
			AddControl(0, _timeLeftLabel);


			AddControl(0, new AnimatedAudioButton(Parent, (e) => Cancel())
			{
				Text = "Cancel",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				Width = 400,
				Height = 100,
				Y = 960,
				X = 20,
			});

			AddControl(0, new AnimatedAudioButton(Parent, (e) => Accept())
			{
				Text = "Accept",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				Width = 400,
				Height = 100,
				Y = 960,
				X = 1500,
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new AcceptView(Parent, _oldSettings, _newSettings)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}
	}
}
