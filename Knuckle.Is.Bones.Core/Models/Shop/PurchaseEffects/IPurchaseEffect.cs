using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	[JsonDerivedType(typeof(UnlockBoardEffect), typeDiscriminator: nameof(UnlockBoardEffect))]
	[JsonDerivedType(typeof(UnlockDiceEffect), typeDiscriminator: nameof(UnlockDiceEffect))]
	[JsonDerivedType(typeof(PointsMultiplierEffect), typeDiscriminator: nameof(PointsMultiplierEffect))]
	[JsonDerivedType(typeof(DiceMultiplierEffect), typeDiscriminator: nameof(DiceMultiplierEffect))]
	[JsonDerivedType(typeof(DiceComboMultiplierEffect), typeDiscriminator: nameof(DiceComboMultiplierEffect))]
	public interface IPurchaseEffect
	{
		public List<Guid> GetReferenceIDs();
	}
}
