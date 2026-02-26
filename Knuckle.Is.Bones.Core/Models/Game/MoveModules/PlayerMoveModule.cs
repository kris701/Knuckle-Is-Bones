using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class PlayerMoveModule : BaseMoveModule
	{
		[JsonConstructor]
		public PlayerMoveModule(Guid opponentID) : base(opponentID)
		{
		}

		internal void SetTargetColumn(int targetColumn) => TargetColumn = targetColumn;

		public override IMoveModule Clone() => new PlayerMoveModule(OpponentID);
	}
}