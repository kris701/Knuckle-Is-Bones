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
			return userData.PurchasedShopItems.Contains(Module.ID);
		}

		public bool Buy<T>(UserSaveDefinition<T> userData) where T : new()
		{
			if (userData.Points < Cost)
				return false;
			if (!userData.PurchasedShopItems.Contains(Module.ID))
				userData.PurchasedShopItems.Add(Module.ID);
			return true;
		}
	}
}
