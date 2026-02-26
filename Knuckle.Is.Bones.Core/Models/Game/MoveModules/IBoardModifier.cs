namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public enum ModifyerType { Mine, Opponent }
	public interface IBoardModifier
	{
		public bool HasModified { get; }
		internal List<ModifyerType> ModifyBoards(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard, int turnIndex);
		internal void Reset();
	}
}
