namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	public class UnlockDiceEffect : IPurchaseEffect
	{
		public Guid ID { get; set; }

		public List<Guid> GetReferenceIDs() => new List<Guid>() { ID };
	}
}
