using System.Text.Json.Serialization;

namespace Knuckle.Is.Bones.Core.Models.Game.OpponentModules
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "OpponentModule")]
    [JsonDerivedType(typeof(PlayerOpponentModule), typeDiscriminator: "Player")]
    [JsonDerivedType(typeof(RandomPositionOpponentModule), typeDiscriminator: "Random")]
    [JsonDerivedType(typeof(DefensiveOpponentModule), typeDiscriminator: "Defensive")]
    public interface IOpponentModule : IGenericClonable<IOpponentModule>
    {
        public Guid OpponentID { get; set; }

        public void SetTargetColumn(DiceDefinition diceValue, BoardDefinition myBoard, BoardDefinition opponentBoard);

        public int GetTargetColumn();
    }
}