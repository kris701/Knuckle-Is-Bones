using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	public class UnlockIdlePointEffect : IPurchaseEffect
	{
		public int PointsToAdd { get; set; }

		public List<Guid> GetReferenceIDs() => new List<Guid>();
	}
}
