using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.GameShopView;
using Knuckle.Is.Bones.OpenGL.Views.HowToPlayView;
using Knuckle.Is.Bones.OpenGL.Views.MainGameView;
using Knuckle.Is.Bones.OpenGL.Views.SettingsMenuView;
using Knuckle.Is.Bones.OpenGL.Views.StartGameView;
using Microsoft.Xna.Framework;

namespace Knuckle.Is.Bones.OpenGL.Views.MainMenuView
{
	public partial class MainMenu : BaseNavigatableView
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
				Y = 25,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Width = 800,
				Height = 264,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Title)
			});

			AddControl(0, new StackPanelControl(new System.Collections.Generic.List<IControl>()
			{
				new AnimatedAudioButton(Parent, (x) =>
				{
					var state = GameSaveHelpers.LoadSaveFile();
					if (state == null || state.FirstOpponent == null)
					{
						GameSaveHelpers.DeleteSave();
						SwitchView(new MainMenu(Parent));
					}
					else
						SwitchView(new MainGame(Parent, state));
				})
				{
					Text = "Continue",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					IsVisible = GameSaveHelpers.DoesSaveExist(),
					Height = GameSaveHelpers.DoesSaveExist() ? 100 : 0
				},
				new AnimatedAudioButton(Parent, (x) => SwitchView(new StartGame(Parent)))
				{
					Text = "New Game",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 100
				},
				new AnimatedAudioButton(Parent, (x) => SwitchView(new GameShop(Parent)))
				{
					Text = "Shop",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 100
				},
				new AnimatedAudioButton(Parent, (x) => SwitchView(new HowToPlay(Parent)))
				{
					Text = "How to Play",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 100
				},
				new AnimatedAudioButton(Parent, (x) => SwitchView(new SettingsMenu(Parent)))
				{
					Text = "Settings",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 100
				},
				new AnimatedAudioButton(Parent, (x) => (Parent as KnuckleBoneWindow).Exit())
				{
					Text = "Exit",
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
					HorizontalAlignment = HorizontalAlignment.Middle,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
					Height = 100
				}
			})
			{
				Y = 300,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Width = 400,
				Height = 500,
				Gap = 25,
			});

			AddControl(0, new LabelControl()
			{
				Text = $"Points: {(Parent as KnuckleBoneWindow).User.Points}",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Top,
				Width = 500,
				Height = 100
			});

			AddControl(0, new LabelControl()
			{
				Text = "Made by Kristian Skov",
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Bottom,
				Width = 500,
				Height = 100
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new MainMenu(Parent)))
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
