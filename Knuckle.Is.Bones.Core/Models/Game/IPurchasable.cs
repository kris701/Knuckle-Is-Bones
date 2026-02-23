using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Game
{
	public interface IPurchasable : IDefinition
	{
		public bool IsPurchasable { get; set; }
	}
}
