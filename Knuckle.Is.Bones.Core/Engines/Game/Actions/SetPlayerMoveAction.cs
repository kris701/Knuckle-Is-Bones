namespace Knuckle.Is.Bones.Core.Engines.Game.Actions
{
	public class SetPlayerMoveAction : IEngineAction
	{
		public int TargetColumn { get; set; }

		public SetPlayerMoveAction(int targetColumn)
		{
			TargetColumn = targetColumn;
		}
	}
}
