using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;

namespace Knuckle.Is.Bones.Core.Models.Shop
{
	public class ShopItemDefinition : IDefinition
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Cost { get; set; }
		public List<IPurchaseEffect> Effects { get; set; }

		public bool HasPurchased(UserSaveDefinition user)
		{
			return user.PurchasedShopItems.Contains(ID);
		}

		public bool CanPurchase(UserSaveDefinition user)
		{
			return user.Points >= Cost;
		}

		public bool Buy(UserSaveDefinition user)
		{
			if (!CanPurchase(user))
				return false;
			user.Points -= Cost;
			foreach (var effect in Effects)
				if (!user.PurchasedShopItems.Contains(effect.ID))
					user.PurchasedShopItems.Add(effect.ID);
			if (!user.PurchasedShopItems.Contains(ID))
				user.PurchasedShopItems.Add(ID);
			user.Save();
			return true;
		}
	}
}
