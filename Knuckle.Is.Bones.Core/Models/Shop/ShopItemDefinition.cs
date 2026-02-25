using Knuckle.Is.Bones.Core.Helpers;
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

		public ShopItemDefinition(Guid iD, string name, string description, int cost, List<IPurchaseEffect> effects)
		{
			ID = iD;
			Name = name;
			Description = description;
			Cost = cost;
			Effects = effects;
		}

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
				foreach (var id in effect.GetReferenceIDs())
					if (!user.PurchasedShopItems.Contains(id))
						user.PurchasedShopItems.Add(id);
			if (!user.PurchasedShopItems.Contains(ID))
				user.PurchasedShopItems.Add(ID);
			UserSaveHelpers.Save(user);
			return true;
		}
	}
}
