using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	[JsonDerivedType(typeof(UnlockBoardEffect), typeDiscriminator: nameof(UnlockBoardEffect))]
	[JsonDerivedType(typeof(UnlockDiceEffect), typeDiscriminator: nameof(UnlockDiceEffect))]
	[JsonDerivedType(typeof(PointsMultiplierEffect), typeDiscriminator: nameof(PointsMultiplierEffect))]
	public interface IPurchaseEffect
	{
		public List<Guid> GetReferenceIDs();
	}
}
