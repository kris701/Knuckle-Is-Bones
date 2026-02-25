using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	public class PlayerMoveModule : IMoveModule
	{
		public Guid OpponentID { get; set; } = Guid.NewGuid();

		private int _targetColumn = 0;

		[JsonConstructor]
		public PlayerMoveModule(Guid opponentID)
		{
			OpponentID = opponentID;
		}

		public void SetTargetColumn(int targetColumn) => _targetColumn = targetColumn;

		public int GetTargetColumn() => _targetColumn;

		public IMoveModule Clone() => new PlayerMoveModule(OpponentID);
	}
}