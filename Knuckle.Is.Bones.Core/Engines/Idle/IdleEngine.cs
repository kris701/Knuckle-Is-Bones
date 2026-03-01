using Knuckle.Is.Bones.Core.Helpers;
using Knuckle.Is.Bones.Core.Models.Saves;
using Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects;
using Knuckle.Is.Bones.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Engines.Idle
{
	public class IdleEngine : IIdleEngine
	{
		private DateTime _lastUpdate = DateTime.UtcNow;

		public void Update(UserSaveDefinition user)
		{
			if (DateTime.UtcNow > _lastUpdate + TimeSpan.FromSeconds(1))
			{
				var shopItemIds = ResourceManager.Shop.GetResources();
				shopItemIds.RemoveAll(x => !user.PurchasedShopItems.Keys.Contains(x));

				int pointsToAdd = 0;
				double pointsToAddMult = 1;
				foreach(var shopItemId in shopItemIds)
				{
					var shopItem = ResourceManager.Shop.GetResource(shopItemId);
					foreach(var effect in shopItem.Effects)
					{
						if (effect is UnlockIdlePointEffect eff)
							for(int i = 0; i < user.PurchasedShopItems[shopItemId]; i++)
								pointsToAdd += eff.PointsToAdd;
						else if (effect is IdlePointMultiplierEffect eff2)
							for (int i = 0; i < user.PurchasedShopItems[shopItemId]; i++)
								pointsToAddMult *= eff2.Multiplier;
					}
				}

				if (pointsToAdd > 0)
				{
					var finalPointsToAdd = (int)(pointsToAdd * pointsToAddMult);
					user.Points += finalPointsToAdd;
					UserSaveHelpers.Save(user);
				}

				_lastUpdate = DateTime.UtcNow;
			}
		}
	}
}
