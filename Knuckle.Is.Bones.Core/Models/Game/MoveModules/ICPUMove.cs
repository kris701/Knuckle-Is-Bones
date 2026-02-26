namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public interface ICPUMove
	{
		internal void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
	}
}
