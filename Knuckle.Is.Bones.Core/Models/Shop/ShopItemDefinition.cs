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

		public bool HasPurchased<T>(UserSaveDefinition<T> userData) where T : new()
		{
			return userData.PurchasedShopItems.Contains(ID);
		}

		public bool CanPurchase<T>(UserSaveDefinition<T> userData) where T : new()
		{
			return userData.Points >= Cost;
		}

		public bool Buy<T>(UserSaveDefinition<T> userData) where T : new()
		{
			if (!CanPurchase(userData))
				return false;
			userData.Points -= Cost;
			foreach (var effect in Effects)
				if (!userData.PurchasedShopItems.Contains(effect.ID))
					userData.PurchasedShopItems.Add(effect.ID);
			if (!userData.PurchasedShopItems.Contains(ID))
				userData.PurchasedShopItems.Add(ID);
			userData.Save();
			return true;
		}
	}
}
