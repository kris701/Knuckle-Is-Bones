using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Shop
{
	public enum ShopItemTypes
	{
		None,
		
		// New items
		NewBoard,
		NewDice,
		NewOpponent,

		// Dice upgrades
		DiceMultiplier,

		// Overall upgrades
		PointMultiplier,

		// Misc
		Multiple
	}
}
