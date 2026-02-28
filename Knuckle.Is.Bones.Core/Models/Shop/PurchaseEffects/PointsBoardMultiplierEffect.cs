namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	public class PointsBoardMultiplierEffect : IPurchaseEffect
	{
		public Guid BoardID { get; set; }
		public double Multiplier { get; set; }
		public List<Guid> GetReferenceIDs() => new List<Guid>();
	}
}
