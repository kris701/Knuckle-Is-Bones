using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseNavigatableView
	{
		private AnimatedTextboxControl _descriptionControl;
		private AnimatedButtonControl _buyItemControl;
		private AnimatedAudioButton? _currentSelectedItem;

		[MemberNotNull(
			nameof(_descriptionControl),
			nameof(_buyItemControl)
			)]
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
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				Text = "Shop",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 10,
				Height = 50,
				FontColor = FontHelpers.SecondaryColor
			});
			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "Purchase new items and upgrades to the game here!",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 50,
				Height = 50,
				FontColor = Color.White
			});
			AddControl(1, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
				Text = $"You have {Parent.User.Points} points",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 100,
				Height = 50,
				FontColor = Color.White
			});

			var width = 600;
			var height = 800;
			var shopIds = ResourceManager.Shop.GetResources();
			var items = new List<ShopItemDefinition>();
			foreach (var id in shopIds)
				items.Add(ResourceManager.Shop.GetResource(id));
			items.RemoveAll(x => x.HasPurchased(Parent.User));
			items = items.OrderBy(x => x.Cost).ToList();

			var textureSet = Parent.Textures.GetTextureSet(TextureHelpers.ShopItem);
			var controlList = new List<AnimatedAudioButton>();
			foreach (var item in items)
			{
				var canBuy = Parent.User.Points >= item.Cost;
				controlList.Add(new AnimatedAudioButton(Parent, (x) => SelectItemToPurchase((AnimatedAudioButton)x))
				{
					TileSet = textureSet,
					FillClickedColor = BasicTextures.GetClickedTexture(),
					FillDisabledColor = BasicTextures.GetBasicRectange(Color.Transparent),
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
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
				ItemsPrPage = 13,
				X = 120,
				Y = 200,
				Width = width,
				Height = height - 50
			};
			AddControl(1, pagehandler);

			if (items.Count == 0)
			{
				AchievementHelper.UnlockAchievement("BuyAllShopItems");
				AddControl(1, new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
					Text = "All items purchased!",
					X = 400,
					Y = 550,
					Height = 50,
					FontColor = FontHelpers.SecondaryColor
				});
			}

			_descriptionControl = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				Margin = 50,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ShopDescription),
				X = pagehandler.X + 10 + width,
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Y = 150,
				Width = 500,
				Height = height,
			};
			AddControl(1, _descriptionControl);
			_buyItemControl = new AnimatedButtonControl(Parent, (x) => PurchaseItem())
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
				FontColor = Color.Gold,
				Text = "Buy",
				ClickSound = SoundEffectHelpers.ShopBuySound,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.Button),
				X = pagehandler.X + 10 + width + 50,
				Y = height + 50,
				Width = 400,
				Height = 50,
				IsVisible = false
			};
			AddControl(1, _buyItemControl);
			AddControl(1, new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				Margin = 50,
				FontColor = Color.White,
				Text = BuildUpgradeList(),
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ShopDescription),
				X = _descriptionControl.X + 10 + _descriptionControl.Width,
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Y = 150,
				Width = 500,
				Height = height,
			});

#if DEBUG
			AddControl(0, new ButtonControl(Parent, (x) => SwitchView(new GameShop(Parent)))
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

		private string BuildUpgradeList()
		{
			var sb = new StringBuilder();

			sb.AppendLine("Current Upgrades");
			sb.AppendLine(" ");

			var shopItemIds = ResourceManager.Shop.GetResources();
			var effects = new List<IPurchaseEffect>();
			foreach (var purcahesedId in Parent.User.PurchasedShopItems.Where(x => shopItemIds.Contains(x)))
				effects.AddRange(ResourceManager.Shop.GetResource(purcahesedId).Effects);

			var allPointMult = effects.Where(x => x is PointsMultiplierEffect).Cast<PointsMultiplierEffect>();
			if (allPointMult.Count() > 0)
			{
				double totalMult = 1;
				foreach (var effect in allPointMult)
					totalMult *= effect.Multiplier;
				sb.AppendLine($"Overall point modifier: {Math.Round(totalMult, 2)}x");
				sb.AppendLine(" ");
			}

			var allDiceMult = effects.Where(x => x is DiceMultiplierEffect).Cast<DiceMultiplierEffect>();
			if (allDiceMult.Count() > 0)
			{
				var groups = allDiceMult.GroupBy(x => x.Number, x => x.Multiplier, (key, g) => new { Key = key, Mults = g.ToList() });
				foreach (var group in groups)
				{
					double totalMult = 1;
					foreach (var mult in group.Mults)
						totalMult *= mult;
					sb.AppendLine($"Dice modifier for {group.Key}'s: {Math.Round(totalMult, 2)}x");
					sb.AppendLine(" ");
				}
			}

			return sb.ToString();
		}
	}
}
