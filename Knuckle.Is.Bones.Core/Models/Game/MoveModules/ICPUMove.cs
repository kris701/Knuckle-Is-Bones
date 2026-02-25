using System;
using System.Collections.Generic;
using System.Text;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	internal interface ICPUMove
	{
		void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
	}
}
