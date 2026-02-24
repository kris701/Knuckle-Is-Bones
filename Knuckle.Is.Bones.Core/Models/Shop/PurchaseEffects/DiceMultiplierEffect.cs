using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Shop.PurchaseEffects
{
	public class DiceMultiplierEffect : IPurchaseEffect
	{
		public int Number { get; set; }
		public double Multiplier { get; set; }
		public List<Guid> GetReferenceIDs() => new List<Guid>();
	}
}
