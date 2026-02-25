using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	internal interface IBoardModifier
	{
		bool ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
	}
}
