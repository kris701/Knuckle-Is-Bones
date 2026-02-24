namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	public class UnlockBoardEffect : IPurchaseEffect
	{
		public Guid ID { get; set; }

		public List<Guid> GetReferenceIDs() => new List<Guid>() { ID };
	}
}
