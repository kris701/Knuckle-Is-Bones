using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using MonoGame.OpenGL.Formatter.Controls;
using MonoGame.OpenGL.Formatter.Helpers;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseKnuckleBoneFadeView
	{
		private AnimatedTextboxControl _descriptionControl;
		private AnimatedButtonControl _buyItemControl;
		private AnimatedAudioButton? _currentSelectedItem;

		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				Text = "Shop",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 10,
				Height = 50,
				FontColor = new Color(217, 68, 144)
			});
			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
				Text = "Purchase new items and upgrades to the game here!",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 50,
				Height = 50,
				FontColor = Color.White
			});
			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx12),
				Text = $"You have {Parent.User.Points} points",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 100,
				Height = 50,
				FontColor = Color.White
			});

			var width = 600;
			var height = 900;
			var shopIds = ResourceManager.Shop.GetResources();
			var items = new List<ShopItemDefinition>();
			foreach (var id in shopIds)
				items.Add(ResourceManager.Shop.GetResource(id));
			items.RemoveAll(x => x.HasPurchased(Parent.User));
			items = items.OrderBy(x => x.Cost).ToList();

			var textureSet = Parent.Textures.GetTextureSet(new System.Guid("ac42399b-fecf-4627-96e8-bb188369dc81"));
			var controlList = new List<AnimatedAudioButton>();
			foreach (var item in items)
			{
				var canBuy = Parent.User.Points >= item.Cost;
				controlList.Add(new AnimatedAudioButton(Parent, (x) => SelectItemToPurchase((AnimatedAudioButton)x))
				{
					TileSet = textureSet,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
					Font = Parent.Fonts.GetFont(FontSizes.Ptx12),
					Text = $"{item.Name} [{item.Cost}]",
					FontColor = canBuy ? Color.White : Color.Gray,
					Alpha = canBuy ? 256 : 100,
					Tag = item,
					IsEnabled = canBuy
				});
			}
			var pagehandler = new PageHandler<AnimatedAudioButton>(this, controlList)
			{
				LeftButtonX = 10,
				LeftButtonY = -50,
				RightButtonX = width - 80,
				RightButtonY = -50,
				ItemsPrPage = 14,
				X = 360,
				Y = 200,
				Width = width,
				Height = height - 50
			};
			AddControl(1, pagehandler);

			if (items.Count == 0)
			{
				SteamUserStats.SetAchievement("BuyAllShopItems");
				AddControl(1, new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontSizes.Ptx16),
					Text = "All items purchased!",
					X = 600,
					Y = 550,
					Height = 50,
					FontColor = new Color(217, 68, 144)
				});
			}

			_descriptionControl = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx8),
				Margin = 50,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(new Guid("0c464e6e-8fcb-4ba9-838d-0b1e5edfca12")),
				X = pagehandler.X + 10 + width,
				Y = 150,
				Width = 500,
				Height = height,
			};
			AddControl(1, _descriptionControl);
			_buyItemControl = new AnimatedButtonControl(Parent, (x) => PurchaseItem())
			{
				Font = Parent.Fonts.GetFont(FontSizes.Ptx12),
				FontColor = Color.Gold,
				Text = "Buy",
				ClickSound = new Guid("83fe89c5-745e-4f76-aa87-02ed76b37b1b"),
				TileSet = Parent.Textures.GetTextureSet(new Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
				X = pagehandler.X + 10 + width + 50,
				Y = height + 50,
				Width = 400,
				Height = 50,
				IsVisible = false
			};
			AddControl(1, _buyItemControl);

			AddControl(0, new AnimatedAudioButton(Parent, (x) => SwitchView(new MainMenu(Parent)))
			{
				Text = "Back",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx24),
				Y = 960,
				X = 20,
				FillClickedColor = BasicTextures.GetClickedTexture(),
				TileSet = Parent.Textures.GetTextureSet(new System.Guid("d9d352d4-ee90-4d1e-98b4-c06c043e6dce")),
				Width = 400,
				Height = 100
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, clicked: (x) => SwitchView(new GameShop(Parent)))
			{
				X = 0,
				Y = 0,
				Width = 50,
				Height = 25,
				Text = "Reload",
				Font = Parent.Fonts.GetFont(FontSizes.Ptx10),
				FillColor = BasicTextures.GetBasicRectange(Color.White),
				FontColor = Color.Black,
				FillClickedColor = BasicTextures.GetBasicRectange(Color.Gray)
			});
#endif

			base.Initialize();
		}
	}
}
