using Knuckle.Is.Bones.Core.Models.Shop;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.OpenGL.Controls;
using Knuckle.Is.Bones.OpenGL.Views.MainMenuView;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.OpenGL.Formatter.Input;
using System;
using System.Text;

namespace Knuckle.Is.Bones.OpenGL.Views.GameShopView
{
	public partial class GameShop : BaseKnuckleBoneFadeView
	{
		public static Guid ID = new Guid("169e9e54-b45f-41d4-9845-f8519d256033");

		private readonly KeyWatcher _escapeKeyWatcher;
		private ShopItemDefinition? _currentShopItem;

		public GameShop(KnuckleBoneWindow parent) : base(parent, ID)
		{
			_escapeKeyWatcher = new KeyWatcher(Keys.Escape, () => SwitchView(new MainMenu(parent)));
			Initialize();
		}

		public override void OnUpdate(GameTime gameTime)
		{
			var keyState = Keyboard.GetState();
			_escapeKeyWatcher.Update(keyState);
		}

		private void SelectItemToPurchase(AnimatedAudioButton sender)
		{
			if (_currentSelectedItem != null)
				_currentSelectedItem.TileSet = Parent.Textures.GetTextureSet(new System.Guid("ac42399b-fecf-4627-96e8-bb188369dc81"));
			_currentSelectedItem = sender;
			sender.TileSet = Parent.Textures.GetTextureSet(new System.Guid("ba6403c6-4af5-4b17-b5ee-62052beb7732"));

			if (sender.Tag is ShopItemDefinition item)
			{
				_currentShopItem = item;
				_descriptionControl.Text = BuildDescription(item);
				if (item.CanPurchase(Parent.User))
					_buyItemControl.IsVisible = true;
			}
		}

		private string BuildDescription(ShopItemDefinition item)
		{
			var sb = new StringBuilder();

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

		private void PurchaseItem()
		{
			if (_currentShopItem == null)
				return;
			if (_currentShopItem.Buy(Parent.User))
			{
				SwitchView(new GameShop(Parent));
			}
		}
	}
}
