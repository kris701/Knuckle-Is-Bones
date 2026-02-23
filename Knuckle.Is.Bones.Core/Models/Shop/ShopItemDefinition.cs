using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Models.Shop.ShopModules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Shop
{
	public class ShopItemDefinition : IDefinition
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Cost { get; set; }
		public IShopModule Module { get; set; }

		public bool HasPurchased<T>(UserSaveDefinition<T> userData) where T : new()
		{
			return userData.PurchasedShopItems.Contains(ID) || userData.PurchasedShopItems.Contains(Module.ID);
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
			if (!userData.PurchasedShopItems.Contains(Module.ID))
				userData.PurchasedShopItems.Add(Module.ID);
			if (!userData.PurchasedShopItems.Contains(ID))
				userData.PurchasedShopItems.Add(ID);
			userData.Save();
			return true;
		}
	}
}
