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
		public int BuyTimes { get; set; }
		public Guid? Requires { get; set; }
		public ShopItemTypes ShopType { get; set; }
		public List<IPurchaseEffect> Effects { get; set; }

		public ShopItemDefinition(Guid iD, string name, string description, int cost, int buyTimes, Guid? requires, ShopItemTypes shopType, List<IPurchaseEffect> effects)
		{
			ID = iD;
			Name = name;
			Description = description;
			Cost = cost;
			BuyTimes = buyTimes;
			Requires = requires;
			ShopType = shopType;
			Effects = effects;
		}

		public bool CanPurchase(UserSaveDefinition user)
		{
			if (Requires != null && !user.PurchasedShopItems.ContainsKey((Guid)Requires))
				return false;
			if (!user.PurchasedShopItems.ContainsKey(ID))
				return true;
			var times = user.PurchasedShopItems[ID];
			if (times >= BuyTimes)
				return false;
			return true;
		}

		public bool IsFullyPurchased(UserSaveDefinition user)
		{
			if (!user.PurchasedShopItems.ContainsKey(ID))
				return false;
			var times = user.PurchasedShopItems[ID];
			if (times == BuyTimes)
				return true;
			return false;
		}

		public bool IsPartiallyPurchased(UserSaveDefinition user)
		{
			if (!user.PurchasedShopItems.ContainsKey(ID))
				return false;
			var times = user.PurchasedShopItems[ID];
			if (times > 0)
				return true;
			return false;
		}

		public bool CanAffort(UserSaveDefinition user)
		{
			return CanPurchase(user) && user.Points >= Cost;
		}

		public bool Buy(UserSaveDefinition user)
		{
			if (!CanAffort(user))
				return false;
			user.Points -= Cost;
			foreach (var effect in Effects)
			{
				foreach (var id in effect.GetReferenceIDs())
				{
					if (!user.PurchasedShopItems.ContainsKey(id))
						user.PurchasedShopItems.Add(id, 0);
					user.PurchasedShopItems[id]++;
				}
			}
			if (!user.PurchasedShopItems.ContainsKey(ID))
				user.PurchasedShopItems.Add(ID, 0);
			user.PurchasedShopItems[ID]++;
			UserSaveHelpers.Save(user);
			return true;
		}
	}
}
