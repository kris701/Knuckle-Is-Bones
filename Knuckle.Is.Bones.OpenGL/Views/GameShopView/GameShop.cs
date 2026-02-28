using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseNavigatableView
	{
		private static Random _rnd = new Random();

		public GameShop(KnuckleBoneWindow parent) : base(parent, new Guid("169e9e54-b45f-41d4-9845-f8519d256033"), new List<int>() { 0, 1 })
		{
			BackAction = () => SwitchView(new MainMenu(Parent));
			Initialize();
		}

		private string BuildDescription(ShopItemDefinition item)
		{
			var sb = new StringBuilder();

			if (item.BuyTimes > 1)
			{
				if (Parent.User.PurchasedShopItems.ContainsKey(item.ID))
					sb.AppendLine($"{Parent.User.PurchasedShopItems[item.ID]}/{item.BuyTimes}");
				else
					sb.AppendLine($"0/{item.BuyTimes}");
			}

			foreach (var effect in item.Effects)
			{
				switch (effect)
				{
					case UnlockDiceEffect m:
						sb.AppendLine("Type: New Dice");
						sb.AppendLine("Cost: " + item.Cost);
						sb.AppendLine(" ");
						sb.AppendLine(item.Description);
						break;
					case UnlockBoardEffect m:
						sb.AppendLine("Type: New Board");
						sb.AppendLine("Cost: " + item.Cost);
						sb.AppendLine(" ");
						sb.AppendLine(item.Description);
						break;
					case PointsMultiplierEffect m:
						sb.AppendLine("Type: Point Multiplier");
						sb.AppendLine("Cost: " + item.Cost);
						sb.AppendLine(" ");
						sb.AppendLine(item.Description);
						break;
					default:
						sb.AppendLine("Type: Unknown");
						sb.AppendLine("Cost: " + item.Cost);
						sb.AppendLine(" ");
						sb.AppendLine(item.Description);
						break;
				}
			}

			return sb.ToString();
		}

		private void PurchaseItem(ShopItemDefinition item)
		{
			if (item.CanAffort(Parent.User) && item.Buy(Parent.User))
				SwitchView(new GameShop(Parent));
		}

		public override void OnUpdate(GameTime gameTime)
		{
			if (InputType == InputTypes.Keyboard)
			{
				if (_keyboardNavigator.Focused is AnimatedAudioButton but && but.Tag is ShopItemDefinition shop)
				{
					_descriptionControl.Text = BuildDescription(shop);
					_descriptionControl.X = but.X + but.Width;
					_descriptionControl.Y = but.Y + but.Height;
					_descriptionControl.IsVisible = true;
				}
			}
			base.OnUpdate(gameTime);
		}
	}
}
