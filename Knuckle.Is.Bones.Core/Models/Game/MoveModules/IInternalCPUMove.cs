namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	internal interface IInternalCPUMove
	{
		void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
	}
}
