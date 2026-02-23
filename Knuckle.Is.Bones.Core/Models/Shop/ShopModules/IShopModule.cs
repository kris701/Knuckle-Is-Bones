using Knuckle.Is.Bones.Core.Models.Saves;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Shop.ShopModules
{
	[JsonDerivedType(typeof(BuyBoardShopModule), typeDiscriminator: nameof(BuyBoardShopModule))]
	[JsonDerivedType(typeof(BuyDiceShopModule), typeDiscriminator: nameof(BuyDiceShopModule))]
	public interface IShopModule
	{
		public Guid ID { get; set; }
	}
}
