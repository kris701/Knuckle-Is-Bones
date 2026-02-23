using Knuckle.Is.Bones.Core.Models.Saves;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Shop.ShopModules
{
	[JsonDerivedType(typeof(UnlockBoardEffect), typeDiscriminator: nameof(UnlockBoardEffect))]
	[JsonDerivedType(typeof(UnlockDiceEffect), typeDiscriminator: nameof(UnlockDiceEffect))]
	public interface IPurchaseEffect
	{
		public Guid ID { get; set; }
	}
}
