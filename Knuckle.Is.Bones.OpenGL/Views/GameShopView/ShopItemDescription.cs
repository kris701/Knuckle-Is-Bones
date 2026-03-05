using FormMatter.OpenGL.Controls;
using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.OpenGL.Helpers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public class ShopItemDescription : CollectionControl
	{
		private readonly LabelControl _cost;
		private readonly StackPanelControl _costPanel;
		private readonly LabelControl _purchaseTimes;
		private readonly TextboxControl _purchageType;
		private readonly TextboxControl _purchaseName;
		private readonly TextboxControl _purchaseDesc;
		private readonly StackPanelControl _mainPanel;
		private readonly StackPanelControl _purchaseTimesPanel;
		private readonly KnuckleBoneWindow _parent;

		public ShopItemDescription(KnuckleBoneWindow parent)
		{
			IsVisible = false;
			_parent = parent;

			Children = new List<IControl>();
			Children.Add(new AnimatedTileControl()
			{
				TileSet = parent.Textures.GetTextureSet(TextureHelpers.ShopDescription)
			});

			var panelItems = new List<IControl>();
			_purchageType = new TextboxControl()
			{
				Text = "",
				Font = parent.Fonts.GetFont(FontHelpers.Ptx12),
				FontColor = FontHelpers.SecondaryColor,
				Height = 30,
				Width = 370,
				WordWrap = TextboxControl.WordWrapTypes.Character
			};
			panelItems.Add(_purchageType);

			_purchaseName = new TextboxControl()
			{
				Text = "",
				Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
				FontColor = FontHelpers.SecondaryColor,
				Height = 30,
				Width = 370,
				WordWrap = TextboxControl.WordWrapTypes.Character
			};
			panelItems.Add(_purchaseName);

			_purchaseDesc = new TextboxControl()
			{
				Text = "",
				Font = parent.Fonts.GetFont(FontHelpers.Ptx8),
				FontColor = FontHelpers.PrimaryColor,
				Height = 60,
				Width = 370,
				WordWrap = TextboxControl.WordWrapTypes.Word
			};
			panelItems.Add(_purchaseDesc);

			_cost = new LabelControl()
			{
				Text = "",
				Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
				FontColor = FontHelpers.SecondaryColor,
				Height = 20,
				FitTextWidth = true
			};
			_costPanel = new StackPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Costs ",
					Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
					FontColor = Color.Gray,
					Height = 20,
					FitTextWidth = true
				},
				_cost,
				new LabelControl()
				{
					Text = " points",
					Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
					FontColor = Color.Gray,
					Height = 20,
					FitTextWidth = true
				},
			})
			{
				Orientation = StackPanelControl.Orientations.Horizontal,
				Width = 400,
				Height = 20,
				Gap = 2,
				X = 60
			};
			panelItems.Add(_costPanel);

			_purchaseTimes = new LabelControl()
			{
				Text = "",
				Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
				FontColor = FontHelpers.SecondaryColor,
				Height = 20,
				FitTextWidth = true
			};
			_purchaseTimesPanel = new StackPanelControl(new List<IControl>()
			{
				new LabelControl()
				{
					Text = "Purchased ",
					Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
					FontColor = Color.Gray,
					Height = 20,
					FitTextWidth = true
				},
				_purchaseTimes,
				new LabelControl()
				{
					Text = " times",
					Font = parent.Fonts.GetFont(FontHelpers.Ptx10),
					FontColor = Color.Gray,
					Height = 20,
					FitTextWidth = true
				},
			})
			{
				Orientation = StackPanelControl.Orientations.Horizontal,
				Width = 400,
				Height = 20,
				Gap = 2,
				X = 60
			};
			panelItems.Add(_purchaseTimesPanel);

			_mainPanel = new StackPanelControl(panelItems)
			{
				Height = 170,
				Width = 370,
				X = 15,
				Y = 15,
				Gap = 2
			};
			Children.Add(_mainPanel);

			Width = 370;
			Height = 170;
		}

		public void SetItem(ShopItemDefinition item)
		{
			var purchaseTimes = 0;
			if (_parent.User.PurchasedShopItems.ContainsKey(item.ID))
				purchaseTimes = _parent.User.PurchasedShopItems[item.ID];

			_purchaseTimes.Text = $"{purchaseTimes}/{item.BuyTimes}";
			_purchageType.Text = GetTextByShopType(item.ShopType);
			_purchaseName.Text = item.Name;
			_purchaseDesc.Text = item.Description;
			_cost.Text = $"{item.Cost}";

			Initialize();
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

				case ShopItemTypes.BoardPointMultiplier:
					return "Board Point Multiplier";

				case ShopItemTypes.NewIdlePoint:
					return "Idle Point Generation";

				case ShopItemTypes.IdlePointMultiplier:
					return "Idle Point Multiplier";

				default:
					return "Other";
			}
		}
	}
}
