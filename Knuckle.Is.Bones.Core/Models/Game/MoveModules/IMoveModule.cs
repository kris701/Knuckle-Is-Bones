using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.MoveModules
{
	[JsonDerivedType(typeof(PlayerMoveModule), typeDiscriminator: nameof(PlayerMoveModule))]
	[JsonDerivedType(typeof(RandomMoveModule), typeDiscriminator: nameof(RandomMoveModule))]
	[JsonDerivedType(typeof(DefensiveMoveModule), typeDiscriminator: nameof(DefensiveMoveModule))]
	[JsonDerivedType(typeof(ComboMoveModule), typeDiscriminator: nameof(ComboMoveModule))]
	public interface IMoveModule : IGenericClonable<IMoveModule>
	{
		public Guid OpponentID { get; set; }

		public void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard);

		public int GetTargetColumn();
	}
}