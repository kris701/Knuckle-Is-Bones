using FormMatter.OpenGL;
using FormMatter.OpenGL.Controls;
using FormMatter.OpenGL.Helpers;
using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.Core.Resources;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using ToolsSharp;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseNavigatableView
	{
		private AnimatedTextboxControl _descriptionControl;
		private AnimatedTextboxControl _overallDescriptionControl;
		private TileControl _coreItem;
		private readonly int _itemDist = 200;

		[MemberNotNull(
			nameof(_descriptionControl),
			nameof(_overallDescriptionControl),
			nameof(_coreItem)
			)]
		public override void Initialize()
		{
			AddControl(0, new TileControl()
			{
				Width = 1920,
				Height = 1080,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});

			AddControl(9, new TileControl()
			{
				Width = 1920,
				Height = 150,
				FillColor = BasicTextures.GetBasicRectange(Color.Black)
			});
			AddControl(10, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx24),
				Text = "Shop",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 10,
				Height = 50,
				FontColor = FontHelpers.SecondaryColor
			});
			AddControl(10, new LabelControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx16),
				Text = "Purchase new items and upgrades to the game here!",
				HorizontalAlignment = HorizontalAlignment.Middle,
				Y = 50,
				Height = 50,
				FontColor = Color.White
			});
			var pointsPanel = new StackPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
					Text = $"You have ",
					FontColor = FontHelpers.PrimaryColor,
					FitTextWidth = true
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
					Text = $"{Parent.User.Points}",
					FontColor = FontHelpers.SecondaryColor,
					FitTextWidth = true
				},
				new LabelControl()
				{
					Font = Parent.Fonts.GetFont(FontHelpers.Ptx12),
					Text = $" points",
					FontColor = FontHelpers.PrimaryColor,
					FitTextWidth = true
				}
			})
			{
				Y = 100,
				Height = 50,
				Width = 350,
				HorizontalAlignment = HorizontalAlignment.Middle,
				Orientation = StackPanelControl.Orientations.Horizontal
			};
			AddControl(10, pointsPanel);

			var shopIds = ResourceManager.Shop.GetResources();
			var items = new List<ShopItemDefinition>();
			foreach (var id in shopIds)
				items.Add(ResourceManager.Shop.GetResource(id));
			CheckAchievement(items);
			BuildTreeFromRoot(items, items.Where(x => x.Requires == null).ToList());

			_descriptionControl = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				Margin = 15,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ShopDescription),
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Width = 200,
				Height = 200,
				IsVisible = false,
			};
			AddControl(6, _descriptionControl);

			_overallDescriptionControl = new AnimatedTextboxControl()
			{
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx8),
				Margin = 50,
				FontColor = Color.White,
				TileSet = Parent.Textures.GetTextureSet(TextureHelpers.ShopOverallDescription),
				WordWrap = TextboxControl.WordWrapTypes.Word,
				Text = BuildUpgradeList(),
				Width = 500,
				Height = 900,
				IsVisible = Parent.User.PurchasedShopItems.Count > 0,
				VerticalAlignment = VerticalAlignment.Middle,
				X = 1400
			};
			AddControl(10, _overallDescriptionControl);

#if DEBUG
			AddControl(10, new ButtonControl(Parent, (x) => SwitchView(new GameShop(Parent)))
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
			foreach (var purchaseId in Parent.User.PurchasedShopItems.Keys.Where(x => shopItemIds.Contains(x)))
				for (int i = 0; i < Parent.User.PurchasedShopItems[purchaseId]; i++)
					effects.AddRange(ResourceManager.Shop.GetResource(purchaseId).Effects);

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

		private void BuildTreeFromRoot(List<ShopItemDefinition> allItems, List<ShopItemDefinition> rootItems)
		{
			_coreItem = new TileControl()
			{
				Width = 1,
				Height = 1,
				X = IWindow.BaseScreenSize.X / 2,
				Y = IWindow.BaseScreenSize.Y / 2
			};
			AddControl(0, _coreItem);

			var existing = new List<IControl>() { _coreItem };
			var index = 0;
			foreach(var item in rootItems)
			{
				var minAngle = (6.28f / rootItems.Count) * index;
				var maxAngle = (6.28f / rootItems.Count) * (index + 1);

				var newItem = CreateButtonControl(item, _coreItem, minAngle, maxAngle, index);
				AddControl(6, newItem);
				existing.Add(newItem);

				var newLine = CreateLineControl(_coreItem, newItem);
				AddControl(5, newLine);
				index++;

				var items = allItems.Where(x => x.Requires == item.ID).ToList();
				if (items.Count > 0)
					BuildTree(newItem, items, allItems, existing, minAngle, maxAngle);
			}

			foreach (var item in GetAll(6))
				if (item is AnimatedAudioButton but)
					_buttonOrigins[but] = new Point((int)but.X, (int)but.Y);
			foreach (var item in GetAll(5))
				if (item is LineControl line)
					_lineOrigins[line] = new LineOrigin(new Point((int)line.X, (int)line.Y), new Point((int)line.X2, (int)line.Y2));
		}

		private void BuildTree(IControl from, List<ShopItemDefinition> items, List<ShopItemDefinition> allItems, List<IControl> existing, float parentMinAngle, float parentMaxAngle)
		{
			var index = 0;
			foreach (var item in items)
			{
				var minAngle = parentMinAngle + ((parentMaxAngle - parentMinAngle) / items.Count) * index;
				var maxAngle = parentMinAngle + ((parentMaxAngle - parentMinAngle) / items.Count) * (index + 1);

				var newItem = CreateButtonControl(item, from, minAngle, maxAngle, index);
				AddControl(6, newItem);
				existing.Add(newItem);

				var newLine = CreateLineControl(from, newItem);
				AddControl(5, newLine);
				index++;

				var subItems = allItems.Where(x => x.Requires == item.ID).ToList();
				if (subItems.Count > 0)
					BuildTree(newItem, subItems, allItems, existing, minAngle, maxAngle);
			}
		}

		private AnimatedAudioButton CreateButtonControl(ShopItemDefinition item, IControl from, float minAngle, float maxAngle, int index)
		{
			var canAffort = item.CanAffort(Parent.User);
			var isFullyPurchased = item.IsFullyPurchased(Parent.User);
			var isPartiallyPurchased = item.IsPartiallyPurchased(Parent.User);

			var newItem = new AnimatedAudioButton(Parent, (s) =>
			{
				if (s.Tag is ShopItemDefinition shopItem)
					PurchaseItem(shopItem);
			})
			{
				Width = 50,
				Height = 50,
				TileSet = isFullyPurchased ? Parent.Textures.GetTextureSet(TextureHelpers.ShopItemPurchased) : (isPartiallyPurchased ? Parent.Textures.GetTextureSet(TextureHelpers.ShopItemPartialPurchased) : Parent.Textures.GetTextureSet(TextureHelpers.ShopItem)),
				Tag = item,
				X = from.X + (float)(Math.Cos(minAngle) * GetDistanceByAngle(minAngle, maxAngle, index)) - 50 / 2,
				Y = from.Y + (float)(Math.Sin(minAngle) * GetDistanceByAngle(minAngle, maxAngle, index)) - 50 / 2,
				Alpha = canAffort || isFullyPurchased ? 256 : (isPartiallyPurchased && canAffort ? 256 : 100),
				FillClickedColor = BasicTextures.GetClickedTexture(),
				Font = Parent.Fonts.GetFont(FontHelpers.Ptx10),
				Text = GetShortTextByShopType(item.ShopType)
			};
			newItem.OnEnter += OnShopItemEnter;
			newItem.OnLeave += OnShopItemLeave;
			return newItem;
		}

		private string GetShortTextByShopType(ShopItemTypes type)
		{
			switch (type)
			{
				case ShopItemTypes.NewBoard:
					return "B";
				case ShopItemTypes.NewDice:
					return "D";
				case ShopItemTypes.NewOpponent:
					return "O";

				case ShopItemTypes.DiceMultiplier:
					return "D*";

				case ShopItemTypes.PointMultiplier:
					return "P*";

				case ShopItemTypes.Multiple:
					return "M";

				default:
					return "?";
			}
		}

		private string GetTextByShopType(ShopItemTypes type)
		{
			switch (type)
			{
				case ShopItemTypes.NewBoard:
					return "New Board";
				case ShopItemTypes.NewDice:
					return "New Dice";
				case ShopItemTypes.NewOpponent:
					return "New Opponent";

				case ShopItemTypes.DiceMultiplier:
					return "Dice Multiplier";

				case ShopItemTypes.PointMultiplier:
					return "Point Multiplier";

				case ShopItemTypes.Multiple:
					return "Multiple";

				default:
					return "Other";
			}
		}

		private LineControl CreateLineControl(IControl from, IControl to)
		{
			var newLine = new LineControl()
			{
				X = from.X + from.Width / 2,
				Y = from.Y + from.Height / 2,
				X2 = to.X + to.Width / 2,
				Y2 = to.Y + to.Height / 2,
				Stroke = BasicTextures.GetBasicRectange(Color.Gray),
				Thickness = 2
			};
			return newLine;
		}

		private int GetDistanceByAngle(float minAngle, float maxAngle, int index)
		{
			if (Math.Abs(maxAngle - minAngle) < 0.2)
			{
				return _itemDist + (_itemDist / 3) * index;
			}
			return _itemDist;
		}

		private void OnShopItemEnter(ButtonControl s)
		{
			if (s.Tag is ShopItemDefinition shopItem)
				UpdateItemDescription(s, shopItem);
		}
		private void OnShopItemLeave(ButtonControl s)
		{
			_descriptionControl.IsVisible = false;
		}

		private void CheckAchievement(List<ShopItemDefinition> items)
		{
			var gotAll = true;
			foreach (var item in items)
			{
				if (!Parent.User.PurchasedShopItems.ContainsKey(item.ID))
				{
					gotAll = false;
					break;
				}
				if (Parent.User.PurchasedShopItems[item.ID] < item.BuyTimes)
				{
					gotAll = false;
					break;
				}
			}
			if (gotAll)
				AchievementHelper.UnlockAchievement("BuyAllShopItems");
		}
	}
}
