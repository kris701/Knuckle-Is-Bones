using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	[JsonDerivedType(typeof(UnlockBoardEffect), typeDiscriminator: nameof(UnlockBoardEffect))]
	[JsonDerivedType(typeof(UnlockDiceEffect), typeDiscriminator: nameof(UnlockDiceEffect))]
	public interface IPurchaseEffect
	{
		public Guid ID { get; set; }
	}
}
