using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
	[JsonDerivedType(typeof(PlayerOpponentModule), typeDiscriminator: nameof(PlayerOpponentModule))]
	[JsonDerivedType(typeof(RandomPositionOpponentModule), typeDiscriminator: nameof(RandomPositionOpponentModule))]
	[JsonDerivedType(typeof(DefensiveOpponentModule), typeDiscriminator: nameof(DefensiveOpponentModule))]
	[JsonDerivedType(typeof(ComboOpponentModule), typeDiscriminator: nameof(ComboOpponentModule))]
	public interface IOpponentModule : IGenericClonable<IOpponentModule>
	{
		public Guid OpponentID { get; set; }

		public void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard);

		public int GetTargetColumn();
	}
}