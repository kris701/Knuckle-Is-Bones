using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public enum ModifyerType { Mine, Opponent }
	internal interface IBoardModifier
	{
		List<ModifyerType> ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
	}
}
